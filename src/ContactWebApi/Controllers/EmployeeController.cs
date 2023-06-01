using Microsoft.AspNetCore.Mvc;

namespace ContactWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _Logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _Logger = logger;
        }
    }
}