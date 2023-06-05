using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Domain.Exceptions;
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
            if (request.DataType == EImportDataType.Unknown)
                throw new NotSupportedImportDataType();

            var validator = new EmployeeDtoValidator();
            var parser = new EmployeeParser(request.DataType);

            foreach (var employee in parser.Parse(request.Text))
            {
                if (!validator.IsValid(employee))
                    throw new RequestModelInvalidException();

                await _Importer.AddAsync(employee, cancellationToken);
            }

            var result = await _Importer.SaveAsync(cancellationToken);

            return result;
        }
    }
}
