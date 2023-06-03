using ContactWebApi.App.Models;

namespace ContactWebApi.App.Parsers
{
    public interface IEmployeeParser
    {
        IEnumerable<EmployeeDto> Parse(string text);

        IAsyncEnumerable<EmployeeDto> Parse(Stream stream);
    }
}
