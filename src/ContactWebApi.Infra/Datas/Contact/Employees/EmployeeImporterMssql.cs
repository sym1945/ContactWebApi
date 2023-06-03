using ContactWebApi.App.Features.Employee.Commands.Import;
using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterMssql : IEmployeeImporter
    {
        public Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
