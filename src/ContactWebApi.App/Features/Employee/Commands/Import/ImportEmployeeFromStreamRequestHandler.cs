using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
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
            // TODO: not suporrted
            if (request.DataType == EImportDataType.Unknown)
                throw new Exception();

            var validator = new EmployeeDtoValidator();
            var parser = new EmployeeParser(request.DataType);

            await foreach (var employee in parser.Parse(request.DataStream))
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
