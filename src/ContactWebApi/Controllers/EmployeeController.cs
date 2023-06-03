using ContactWebApi.App.Features.Employee.Queries.GetPage;
using ContactWebApi.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _Logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _Logger = logger;
        }

        [HttpGet]
        public IAsyncEnumerable<EmployeeDto> GetEmployeesPage([FromQuery] GetEmployeePageRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{name}")]
        public Task<IList<EmployeeDto>> GetEmployeeByName(string name)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> ImportEmployees()
        {
            throw new NotImplementedException();
        }
    }
}