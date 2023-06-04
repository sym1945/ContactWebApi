using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
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
            // TODO: not suporrted
            if (request.DataType == EImportDataType.Unknown)
                throw new Exception();

            var validator = new EmployeeDtoValidator();
            var parser = new EmployeeParser(request.DataType);

            foreach (var employee in parser.Parse(request.Text))
            {
                // TODO: employee validataion
                if (!validator.IsValid(employee))
                    throw new Exception();

                await _Importer.AddAsync(employee, cancellationToken);
            }

            var result = await _Importer.SaveAsync(cancellationToken);

            return result;
        }
    }
}
