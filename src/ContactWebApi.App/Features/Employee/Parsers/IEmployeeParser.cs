using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Parsers
{
    public interface IEmployeeParser
    {
        IEnumerable<EmployeeDto> Parse(string text);

        IAsyncEnumerable<EmployeeDto> Parse(Stream stream);
    }
}
