using ContactWebApi.App.Models;

namespace ContactWebApi.App.Parsers
{
    public interface IEmployeeParser
    {
        IList<Employee> Parse(string text);

        IAsyncEnumerable<Employee> Parse(Stream stream);
    }
}
