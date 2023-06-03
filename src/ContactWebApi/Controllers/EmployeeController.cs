using ContactWebApi.App.Features.Employee.Queries.GetByName;
using ContactWebApi.App.Features.Employee.Queries.GetPage;
using ContactWebApi.App.Models;
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
        public Task<IActionResult> ImportEmployees()
        {
            throw new NotImplementedException();
        }
    }
}