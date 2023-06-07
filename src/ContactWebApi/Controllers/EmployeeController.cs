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
        /// Employee ������ ��ȸ
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Employee ������ ���� ����</response>
        /// <response code="400">page �ʵ尡 �������� �ʰų� 1���� ����. �Ǵ�, pageSize �ʵ尡 �������� �ʰų� 1 ~ 100 ������ ���� �ƴ�</response>
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
        /// Employee �̸����� ��ȸ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// ���������� ����Ͽ� �迭�� ��ȯ
        /// </remarks>
        /// <response code="200">Employee ���� ���� (�迭)</response>
        /// <response code="400">name �ʵ尡 �������� �ʰų� 256�ڸ� �ʰ���</response>
        /// <response code="404">name�� ��ġ�ϴ� Employee�� �������� ����</response>
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
        /// Employee Id�� ��ȸ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Employee ���� ����</response>
        /// <response code="400">��ȿ���� ���� Id ��</response>
        /// <response code="404">�ش�Ǵ� Id�� Employee�� �������� ����</response>
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
        /// Employee GroupId�� ��ȸ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// POST ������� ������ ������ Import �� GroupId�� ����
        /// 
        /// �ش� GroupId�� ��Ī�Ǵ� Employee �迭�� ��ȯ
        /// </remarks>
        /// <response code="200">Employee ���� ���� (�迭)</response>
        /// <response code="400">��ȿ���� ���� GroupId ��</response>
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
        /// Employee ������ Import
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// CSV, JSON ������ Employee ������ Import
        /// 
        /// �����Ǵ� Request Content-Type
        /// - application/json
        /// - text/csv
        /// - multipart/form-data
        /// - application/x-www-form-urlencoded
        /// 
        /// ��
        /// - Form File�� ���� ��: Requst Content-Type = 'multipart/form-data', File Content-Type = 'application/json' or 'text/csv'
        /// - Form Text�� ���� ��: Requst Content-Type = 'application/x-www-form-urlencoded'
        /// - RawData(Body)�� ���� ��: Requst Content-Type = 'application/json' or 'text/csv'
        /// 
        /// </remarks>
        /// <response code="201">���ҽ� ���� ����. ���� ���� �� Group ���ҽ� Uri ��ȯ</response>
        /// <response code="204">�� �����Ͱ� ���� �ƹ��� �������� ����</response>
        /// <response code="400">Json �Ǵ� CSV ���°� ��ȿ���� �ʰų�, �Ľ̵� Employee �����Ͱ� ��ȿ���� ����</response>
        /// <response code="409">������ �� �̹� ��ϵ� Email �Ǵ� Tel�� ����</response>
        /// <response code="415">�������� �ʴ� ContentType</response>
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
                // 1. File �˻�
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    contentType = file.ContentType;
                    stream = file.OpenReadStream();
                }
                // 2. Text �˻�
                else
                {
                    text = Request.Form.First().Value[0];
                }
            }
            else
            {
                // 1. Body �˻�
                contentType = Request.ContentType;
                stream = Request.Body;
            }

            if (stream != null && contentType != null)
            {
                // stream���� ������ parse
                var dataType = ImportDataTypeConverter.CovertFrom(contentType);

                result = await _Mediator.Send(new ImportEmployeeFromStreamRequest(dataType, stream));
            }
            else
            {
                // text���� ������ parse. text ���� �𸣴� Json -> Csv ������ �˻�
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