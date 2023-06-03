using ContactWebApi.App.Models;

namespace ContactWebApi.App.Parsers
{
    public class EmployeeParser : IEmployeeParser
    {
        public IEnumerable<EmployeeDto> Parse(string text)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
