using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Attributes;
using ContactWebApi.Constants;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Helpers;
using ContactWebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static ContactWebApi.App.Constants.Employee.EmployeeFiled;

namespace ContactWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _Mediator;
        private readonly ILogger<EmployeeController>? _Logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController>? logger = null)
        {
            _Mediator = mediator;
            _Logger = logger;
        }

        /// <summary>
        /// Employee 페이지 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Employee 페이지 정보 리턴</response>
        /// <response code="400">page 필드가 존재하지 않거나 1보다 작음. 또는, pageSize 필드가 존재하지 않거나 1 ~ 100 사이의 값이 아님</response>
        [HttpGet]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEmployeePageResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetEmployeesPage([FromQuery] GetEmployeePageRequest request)
        {
            request.SetPageUriCreator(new PageUriCreator((page, pageSize) =>
                new Uri(Url.ActionLink(action: nameof(GetEmployeesPage), values: new { page, pageSize })!)
            ));

            var result = await _Mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Employee 이름으로 조회
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// 동명이인을 고려하여 배열로 반환
        /// </remarks>
        /// <response code="200">Employee 정보 리턴 (배열)</response>
        /// <response code="400">name 필드가 존재하지 않거나 256자를 초과함</response>
        /// <response code="404">name과 일치하는 Employee가 존재하지 않음</response>
        [HttpGet("{name}")]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeLinkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeesByName([MaxLength(NameMax)] string name)
        {
            var result = new List<EmployeeLinkDto>();
            var stream = _Mediator.CreateStream(new GetEmployeeByNameRequest { EmployeeName = name });
            await foreach (var employee in stream)
            {
                employee.Link = new Uri(Url.ActionLink(action: nameof(GetEmployeeById), values: new { id = employee.Id })!);
                result.Add(employee);
            }
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Employee Id로 조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Employee 정보 리턴</response>
        /// <response code="400">유효하지 않은 Id 값</response>
        /// <response code="404">해당되는 Id의 Employee가 존재하지 않음</response>
        [HttpGet("id/{id}")]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _Mediator.Send(new GetEmployeeByIdRequest { EmployeeId = id });
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Employee GroupId로 조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// POST 명령으로 복수의 데이터 Import 시 GroupId를 생성
        /// 
        /// 해당 GroupId와 매칭되는 Employee 배열을 반환
        /// </remarks>
        /// <response code="200">Employee 정보 리턴 (배열)</response>
        /// <response code="400">유효하지 않은 GroupId 값</response>
        [HttpGet("group/{id}")]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async IAsyncEnumerable<EmployeeDto> GetEmployeesByGroupId(int id)
        {
            var stream = _Mediator.CreateStream(new GetEmployeeByGroupIdRequest { GroupId = id });

            await foreach (var employee in stream)
                yield return employee;
        }

        /// <summary>
        /// Employee 데이터 Import
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// CSV, JSON 형식의 Employee 데이터 Import
        /// 
        /// 지원되는 Request Content-Type
        /// - application/json
        /// - text/csv
        /// - multipart/form-data
        /// - application/x-www-form-urlencoded
        /// 
        /// 예
        /// - Form File로 전달 시: Requst Content-Type = 'multipart/form-data', File Content-Type = 'application/json' or 'text/csv'
        /// - Form Text로 전달 시: Requst Content-Type = 'application/x-www-form-urlencoded'
        /// - RawData(Body)로 전달 시: Requst Content-Type = 'application/json' or 'text/csv'
        /// 
        /// </remarks>
        /// <response code="201">리소스 생성 성공. 생성 개수 및 Group 리소스 Uri 반환</response>
        /// <response code="204">빈 데이터가 들어와 아무런 변경점이 없음</response>
        /// <response code="400">Json 또는 CSV 형태가 유효하지 않거나, 파싱된 Employee 데이터가 유효하지 않음</response>
        /// <response code="409">데이터 중 이미 등록된 Email 또는 Tel이 존재</response>
        /// <response code="415">지원되지 않는 ContentType</response>
        [HttpPost]
        [ImportEmployeesConsumes]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportEmployeeResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> ImportEmployees()
        {
            EmployeeImportResult? result = null;
            Stream? stream = null;
            string? contentType = null;
            string text = string.Empty;

            if (Request.HasFormContentType)
            {
                // 1. File 검색
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    contentType = file.ContentType;
                    stream = file.OpenReadStream();
                }
                // 2. Text 검색
                else
                {
                    text = Request.Form.First().Value[0];
                }
            }
            else
            {
                // 1. Body 검색
                contentType = Request.ContentType;
                stream = Request.Body;
            }

            if (stream != null && contentType != null)
            {
                // stream에서 데이터 parse
                var dataType = ImportDataTypeConverter.CovertFrom(contentType);

                result = await _Mediator.Send(new ImportEmployeeFromStreamRequest(dataType, stream));
            }
            else
            {
                // text에서 데이터 parse. text 형식 모르니 Json -> Csv 순서로 검사
                try
                {
                    if (text.IndexOf('[') >= 0)
                        result = await _Mediator.Send(new ImportEmployeeFromTextRequest(EImportDataType.Json, text));
                }
                catch (InvalidModelException)
                {
                }

                if (result == null)
                    result = await _Mediator.Send(new ImportEmployeeFromTextRequest(EImportDataType.Csv, text));
            }

            if (result.Count == 0)
                return NoContent();

            var resourceUri = new Uri(Url.ActionLink(action: nameof(GetEmployeesByGroupId), values: new { id = result.GroupId })!);

            var response = new ImportEmployeeResponse
            {
                CreatedCount = result.Count,
                ResourceUri = resourceUri
            };

            return Created(resourceUri, response);
        }
    }

}