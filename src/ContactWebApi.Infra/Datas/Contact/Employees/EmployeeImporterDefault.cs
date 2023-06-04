using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterDefault : IEmployeeImporter
    {
        private readonly ContactDbContext _Context;
        private EmployeeGroup? _Group;
        private IDbContextTransaction? _Transaction;

        public EmployeeImporterDefault(ContactDbContext context)
        {
            _Context = context;
        }

        public async Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            // TODO: Thread-safe...
            if (_Group == null)
            {
                _Transaction = await _Context.Database.BeginTransactionAsync();
                _Group = await _Context.CreateEmployeeGroupAsync(cancelToken);
            }

            await _Context.Employees.AddAsync(new Employee
            {
                Name = employee.Name,
                Email = employee.Email,
                Tel = employee.Tel,
                Joined = employee.Joined.ToDateTime(TimeOnly.MinValue),
                GroupId = _Group.Id
            });
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            if (_Group == null)
                throw new Exception(); // TODO:

            var count = await _Context.SaveChangesAsync(cancelToken);

            if (count > 0 && _Transaction != null)
                await _Transaction.CommitAsync();

            return new EmployeeImportResult(_Group.Id, count);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_Transaction != null)
                    await _Transaction.DisposeAsync();
            }
            catch
            {
            }
        }
    }
}
