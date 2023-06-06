using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Text;


namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterMssql : IEmployeeImporter
    {
        private readonly ILogger<EmployeeImporterMssql>? _Logger;
        private readonly ContactDbContext _Context;

        private string? _FilePath;
        private EmployeeGroup? _Group;
        private StreamWriter? _Writer;
        private IDbContextTransaction? _Transaction;

        public EmployeeImporterMssql(ContactDbContext context, ILogger<EmployeeImporterMssql>? logger = null)
        {
            _Context = context;
            _Logger = logger;
        }

        public async Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            // TODO: Thread-safe...
            if (_Group == null)
            {
                _Transaction = await _Context.Database.BeginTransactionAsync(cancelToken);
                _Group = await _Context.CreateEmployeeGroupAsync(cancelToken);
            }

            if (_Writer == null)
            {
                _FilePath = Path.GetTempFileName();
                _Writer = new StreamWriter(_FilePath, false, Encoding.Unicode);
            }

            await _Writer.WriteLineAsync($"{employee.Name}\t{employee.Email}\t{employee.Tel}\t{employee.Joined:yyyy-MM-dd}\t{_Group.Id}");
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            if (_Group == null || _FilePath == null)
                return new EmployeeImportResult(0, 0);

            _Writer?.Close();

            var query = CreateBulkInsertQuery();

            var count = await _Context.Database.ExecuteSqlRawAsync(query, cancelToken);

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

            try
            {
                if (_Writer != null)
                    await _Writer.DisposeAsync();

                if (_FilePath != null)
                    File.Delete(_FilePath);
            }
            catch
            {
            }
        }

        private string CreateBulkInsertQuery()
        {
            var formatFilePath = Path.Combine(AppContext.BaseDirectory, @"Resources\Infra\Employees.fmt");
            var database = _Context.Database.GetDbConnection().Database;
            var schema = _Context.Employees.EntityType.GetSchema() ?? "dbo";
            var table = _Context.Employees.EntityType.GetTableName();

            return @$"BULK INSERT [{database}].[{schema}].[{table}] FROM '{_FilePath}' WITH (FORMATFILE = '{formatFilePath}')";
        }

    }
}
