using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
using MediatR;


namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromStreamRequestHandler : IRequestHandler<ImportEmployeeFromStreamRequest, int>
    {
        private readonly IEmployeeImporter _Importer;

        public ImportEmployeeFromStreamRequestHandler(IEmployeeImporter importer)
        {
            _Importer = importer;
        }

        public async Task<int> Handle(ImportEmployeeFromStreamRequest request, CancellationToken cancellationToken)
        {
            // TODO: not suporrted
            if (request.DataType == EImportDataType.Unknown)
                throw new Exception();

            var parser = new EmployeeParser(request.DataType);

            await foreach (var employee in parser.Parse(request.DataStream))
            {
                // TODO: employee validataion

                await _Importer.AddAsync(employee, cancellationToken);
            }

            var result = await _Importer.SaveAsync(cancellationToken);

            return result.Count;
        }
    }
}
