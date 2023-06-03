using ContactWebApi.App.Parsers;
using ContactWebApi.Domain.Enums;
using MediatR;


namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromStreamRequestHandler : IRequestHandler<ImportEmployeeFromStreamRequest, int>
    {
        public ImportEmployeeFromStreamRequestHandler()
        {
        }

        public async Task<int> Handle(ImportEmployeeFromStreamRequest request, CancellationToken cancellationToken)
        {
            int totalCount = 0;

            switch (request.DataType)
            {
                case EImportDataType.Json:
                    {
                        var parser = new EmployeeJsonParser();
                        await foreach (var employee in parser.Parse(request.DataStream))
                        {
                            totalCount++;
                        }
                        break;
                    }
                case EImportDataType.Csv:
                    {
                        var parser = new EmployeeCsvParser();
                        await foreach (var employee in parser.Parse(request.DataStream))
                        {
                            totalCount++;
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception();
                    }
            }

            return totalCount;
        }
    }
}
