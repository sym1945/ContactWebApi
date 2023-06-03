using ContactWebApi.App.Parsers;
using ContactWebApi.Domain.Enums;
using MediatR;


namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromTextRequestHandler : IRequestHandler<ImportEmployeeFromTextRequest, int>
    {
        public ImportEmployeeFromTextRequestHandler()
        {
        }

        public Task<int> Handle(ImportEmployeeFromTextRequest request, CancellationToken cancellationToken)
        {
            int totalCount = 0;
            try
            {
                var parser = new EmployeeJsonParser();
                foreach (var employee in parser.Parse(request.Text))
                {
                    totalCount++;
                }

                return Task.FromResult(totalCount);
            }
            catch (Exception)
            {
            }

            totalCount = 0;
            try
            {
                var parser = new EmployeeCsvParser();
                foreach (var employee in parser.Parse(request.Text))
                {
                    totalCount++;
                }

                return Task.FromResult(totalCount);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
