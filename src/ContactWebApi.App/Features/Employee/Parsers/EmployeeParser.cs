using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Domain.Exceptions;

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
                default: throw new NotImplementedException();
            }
        }

        public IEnumerable<EmployeeDto> Parse(string text)
        {
            try
            {
                return _Parser.Parse(text);
            }
            catch
            {
                throw new RequestModelInvalidException();
            }
            
        }

        public IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            try
            {
                return _Parser.Parse(stream);
            }
            catch (Exception)
            {
                throw new RequestModelInvalidException();
            }
        }

    }
}
