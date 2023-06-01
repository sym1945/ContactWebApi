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
        public Task<List<Employee>> GetEmployees([FromQuery] EmployeeRequest request)
        {
            return Task.FromResult(new List<Employee>());
        }

        [HttpGet("{name}")]
        public Task<List<Employee>> GetEmployeeDetail(string name)
        { 
            return Task.FromResult(new List<Employee>());
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployees()
        {
            return CreatedAtAction(nameof(GetEmployees), null);
        }
    }
}