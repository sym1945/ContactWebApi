using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Attributes;
using ContactWebApi.Constants;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Helpers;
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
                new Uri(Url.ActionLink(action: nameof(GetEmployeesPage), values: new { page, pageSize})!)
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> ImportEmployees()
        {
            //ValidationProblem()

            EmployeeImportResult? result = null;

            if (Request.HasFormContentType)
            {
                // 1. File 검색
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.First();
                    var dataType = ImportDataTypeConverter.CovertFrom(file.ContentType);
                    if (dataType == EImportDataType.Unknown)
                        return BadRequest();

                    result = await _Mediator.Send(new ImportEmployeeFromStreamRequest(dataType, file.OpenReadStream()));
                }
                // 2. Text 검색
                else
                {
                    if (Request.Form.Count > 0)
                    {
                        var text = Request.Form.First().Value[0];

                        result = await _Mediator.Send(new ImportEmployeeFromTextRequest(text));
                    }
                }
            }
            else
            {
                // 1. Body 검색
                var dataType = ImportDataTypeConverter.CovertFrom(Request.ContentType);
                if (dataType == EImportDataType.Unknown)
                    return BadRequest();

                result = await _Mediator.Send(new ImportEmployeeFromStreamRequest(dataType, Request.Body));
            }

            if (result == null)
            {
                // TODO:
                throw new Exception();
            }

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