using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public interface IEmployeeImporter : IDisposable
    {
        Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default);

        Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default);

        void Clear();
    }
}
