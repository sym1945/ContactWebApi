using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Domain.Models;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class ImportEmployeeFromStreamRequestHandler : IRequestHandler<ImportEmployeeFromStreamRequest, EmployeeImportResult>
    {
        private readonly IEmployeeImporter _Importer;

        public ImportEmployeeFromStreamRequestHandler(IEmployeeImporter importer)
        {
            _Importer = importer;
        }

        public async Task<EmployeeImportResult> Handle(ImportEmployeeFromStreamRequest request, CancellationToken cancellationToken)
        {
            var validator = new EmployeeDtoValidator();
            var parser = new EmployeeParser(request.DataType);

            await foreach (var employee in parser.Parse(request.DataStream))
            {
                if (!validator.IsValid(employee, out ModelError[] errors))
                    throw new InvalidModelException(modelErrors: errors);

                await _Importer.AddAsync(employee, cancellationToken);
            }

            var result = await _Importer.SaveAsync(cancellationToken);

            return result;
        }
    }
}
