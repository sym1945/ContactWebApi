using AutoMapper;
using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterDefault : IEmployeeImporter
    {
        private readonly ContactDbContext _Context;
        private readonly IMapper _Mapper;
        private EmployeeGroup? _Group;
        private IDbContextTransaction? _Transaction;
        

        public EmployeeImporterDefault(ContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        public async Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            // TODO: Thread-safe...
            if (_Group == null)
            {
                _Transaction = await _Context.Database.BeginTransactionAsync(cancelToken);
                _Group = await _Context.CreateEmployeeGroupAsync(cancelToken);
            }

            var entity = _Mapper.Map<Employee>(employee);
            entity.GroupId = _Group.Id;

            await _Context.Employees.AddAsync(entity);
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            if (_Group == null)
                return new EmployeeImportResult(0, 0);

            var count = await _Context.SaveChangesAsync(cancelToken);

            if (count > 0 && _Transaction != null)
                await _Transaction.CommitAsync(cancelToken);

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
