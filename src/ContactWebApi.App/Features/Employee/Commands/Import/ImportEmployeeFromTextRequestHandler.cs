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
            var validator = new EmployDtoValidator();

            int totalCount = 0;
            try
            {
                var parser = new EmployeeParser(EImportDataType.Json);
                foreach (var employee in parser.Parse(request.Text))
                {
                    // TODO: employee validataion
                    if (!validator.IsValid(employee))
                        throw new Exception();
                    
                    await _Importer.AddAsync(employee, cancellationToken);
                    totalCount++;
                }

                var result = await _Importer.SaveAsync();

                return result;
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
                    if (!validator.IsValid(employee))
                        throw new Exception();

                    await _Importer.AddAsync(employee, cancellationToken);
                    totalCount++;
                }

                var result = await _Importer.SaveAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
