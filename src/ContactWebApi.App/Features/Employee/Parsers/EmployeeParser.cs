using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Enums;

namespace ContactWebApi.App.Features.Employee.Parsers
{
    public class EmployeeParser : IEmployeeParser
    {
        private readonly IEmployeeParser _Parser;

        public EmployeeParser(EImportDataType dataType)
        {
            switch (dataType)
            {
                case EImportDataType.Csv: _Parser = new EmployeeCsvParser(); break;
                case EImportDataType.Json: _Parser = new EmployeeJsonParser(); break;
                // TODO: not supported
                default: throw new Exception();
            }
        }

        public IEnumerable<EmployeeDto> Parse(string text)
        {
            return _Parser.Parse(text);
        }

        public IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            return _Parser.Parse(stream);
        }
    }
}
