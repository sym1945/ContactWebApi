using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Entities;


namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterDefault : IEmployeeImporter
    {
        private readonly ContactDbContext _Context;
        private int? _GroupId;

        public EmployeeImporterDefault(ContactDbContext context)
        {
            _Context = context;
        }

        private async Task<int> CrateGroupIdAsync(CancellationToken cancelToken = default)
        {
            var entry = await _Context.EmployeeGroups.AddAsync(new EmployeeGroup { CreateTime = DateTime.UtcNow }, cancelToken);
            await _Context.SaveChangesAsync(cancelToken);

            return entry.Entity.Id;
        }

        public async Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            if (!_GroupId.HasValue)
                _GroupId = await CrateGroupIdAsync(cancelToken);

            await _Context.Employees.AddAsync(new Employee
            {
                Name = employee.Name,
                Email = employee.Email,
                Tel = employee.Tel,
                Joined = employee.Joined.ToDateTime(TimeOnly.MinValue),
                GroupId = _GroupId.Value
            });
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            if (!_GroupId.HasValue)
                throw new Exception(); // TODO:

            var count = await _Context.SaveChangesAsync(cancelToken);

            return new EmployeeImportResult(_GroupId.Value, count);
        }

        public void Clear()
        {
            // TODO:
        }

        public void Dispose()
        {
            _Context.Dispose();
        }

    }
}
