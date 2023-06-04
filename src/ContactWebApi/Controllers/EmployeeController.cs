using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;


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
        [Produces(MediaTypeNames.Application.Json)]
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
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeByName(string name)
        {
            var result = await _Mediator.Send(new GetEmployeeByNameRequest { EmployeeName = name });
            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("id/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeesById(int id)
        {
            var result = await _Mediator.Send(new GetEmployeeByIdRequest { EmployeeId = id });
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("group/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeDto>))]
        public async IAsyncEnumerable<EmployeeDto> GetEmployeeByGroupId(int id)
        {
            var employeeStream = _Mediator.CreateStream(new GetEmployeeByGroupIdRequest { GroupId = id });

            await foreach (var employee in employeeStream)
                yield return employee;
        }

        [HttpPost]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
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

            var resourceUri = new Uri(Url.ActionLink(action: nameof(GetEmployeeByGroupId), values: new { id = result.GroupId })!);

            var response = new ImportEmployeeResponse
            {
                CreatedCount = result.Count,
                ResourceUri = resourceUri
            };

            return Created(resourceUri, response);
        }
    }

}