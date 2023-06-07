using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Domain.Models;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class ImportEmployeeFromTextRequestHandler : IRequestHandler<ImportEmployeeFromTextRequest, EmployeeImportResult>
    {
        private readonly IEmployeeImporter _Importer;

        public ImportEmployeeFromTextRequestHandler(IEmployeeImporter importer)
        {
            _Importer = importer;
        }

        public async Task<EmployeeImportResult> Handle(ImportEmployeeFromTextRequest request, CancellationToken cancellationToken)
        {
            var parser = new EmployeeParser(request.DataType);
            var validator = new EmployeeDtoValidator();

            foreach (var employee in parser.Parse(request.Text))
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
