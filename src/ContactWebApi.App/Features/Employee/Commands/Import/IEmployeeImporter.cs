using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public interface IEmployeeImporter : IAsyncDisposable
    {
        Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default);

        Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default);
    }
}
