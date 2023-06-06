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


namespace ContactWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _Mediator;
        private readonly ILogger<EmployeeController> _Logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _Mediator = mediator;
            _Logger = logger;
        }

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

        [HttpGet("{name}")]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeLinkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeesByName(string name)
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

        [HttpPost]
        [ImportEmployeesConsumes]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImportEmployeeResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
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