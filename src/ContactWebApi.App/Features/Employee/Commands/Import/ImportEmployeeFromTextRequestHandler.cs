using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
using MediatR;


namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromTextRequestHandler : IRequestHandler<ImportEmployeeFromTextRequest, int>
    {
        private readonly IEmployeeImporter _Importer;

        public ImportEmployeeFromTextRequestHandler(IEmployeeImporter importer)
        {
            _Importer = importer;
        }

        public async Task<int> Handle(ImportEmployeeFromTextRequest request, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            try
            {
                var parser = new EmployeeParser(EImportDataType.Json);
                foreach (var employee in parser.Parse(request.Text))
                {
                    // TODO: employee validataion
                    await _Importer.AddAsync(employee, cancellationToken);
                    totalCount++;
                }

                var result = await _Importer.SaveAsync();

                return result.Count;
            }
            catch (Exception)
            {
            }

            totalCount = 0;
            try
            {
                var parser = new EmployeeParser(EImportDataType.Csv);
                foreach (var employee in parser.Parse(request.Text))
                {
                    // TODO: employee validataion
                    await _Importer.AddAsync(employee, cancellationToken);
                    totalCount++;
                }

                var result = await _Importer.SaveAsync();

                return result.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
