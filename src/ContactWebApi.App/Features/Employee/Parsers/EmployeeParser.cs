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
            var has = true;
            var enumerator = _Parser.Parse(text).GetEnumerator();

            do
            {
                try
                {
                    has = enumerator.MoveNext();
                }
                catch
                {
                    throw new RequestModelInvalidException();
                }

                if (has)
                    yield return enumerator.Current;
            }
            while (has);
        }

        public async IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            var has = true;
            var enumerator = _Parser.Parse(stream).GetAsyncEnumerator();

            do
            {
                try
                {
                    has = await enumerator.MoveNextAsync();
                }
                catch
                {
                    throw new RequestModelInvalidException();
                }

                if (has)
                    yield return enumerator.Current;
            }
            while (has);
        }

    }
}
