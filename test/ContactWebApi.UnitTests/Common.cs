using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.App.Features.Employee.Queries;
using System.Text;

namespace ContactWebApi.UnitTests
#nullable disable
{
    public static class Common
    {
        public static EmployeeDto CreateEmployee(string name = "name", string email = "name@gmail.com", string tel = "010-1234-5678", string joined = "2023-06-06")
        {
            var dto = new EmployeeDto()
            {
                Name = name,
                Email = email,
                Tel = tel,
                Joined = DateOnly.TryParse(joined, out DateOnly result) ? result : null
            };

            return dto;
        }

        public static GetEmployeePageRequest CreateEmployeePageRequest(int? page = 1, int? pageSize = 1)
        {
            return new GetEmployeePageRequest { Page = page, PageSize = pageSize };
        }

        public static string CreateString(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; ++i)
                sb.Append("x");

            return sb.ToString();
        }
    }
}
