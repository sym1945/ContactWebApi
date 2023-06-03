using ContactWebApi.App.Features.Employee.Commands.Import;
using ContactWebApi.App.Features.Employee.Queries.GetByName;
using ContactWebApi.App.Features.Employee.Queries.GetPage;
using ContactWebApi.App.Models;
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
        public IAsyncEnumerable<EmployeeDto> GetEmployeesPage([FromQuery] GetEmployeePageRequest request)
        {
            return _Mediator.CreateStream(request);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetEmployeeByName(string name)
        {
            var result = await _Mediator.Send(new GetEmployeeByNameRequest { EmployeeName = name });
            if (result.Count == 0)
                return NotFound(name);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ImportEmployees()
        {
            int result = 0;

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

            return Ok(result);
        }
    }
}