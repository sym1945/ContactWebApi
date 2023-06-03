using ContactWebApi.App.Features.Employee.Commands.Import;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterMssql : IEmployeeImporter
    {
        private ILogger<EmployeeImporterMssql> _Logger;
        private readonly ContactDbContext _Context;
        private int? _GroupId;
        private string? _FilePath;
        private StreamWriter? _Writer;

        public EmployeeImporterMssql(ContactDbContext context, ILogger<EmployeeImporterMssql> logger)
        {
            _Context = context;
            _Logger = logger;
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

            if (_Writer == null)
            {
                _FilePath = Path.GetTempFileName();
                _Writer = new StreamWriter(_FilePath, false, Encoding.Unicode);
            }

            await _Writer.WriteLineAsync($"{employee.Name}\t{employee.Email}\t{employee.Tel}\t{employee.Joined.ToString("yyyy-MM-dd")}\t{_GroupId.Value}");
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            if (!_GroupId.HasValue || _FilePath == null)
            {
                // TODO:
                throw new Exception();
            }

            _Writer?.Close();

            var formatFilePath = Path.Combine(AppContext.BaseDirectory, @"Resources\Infra\Employees.fmt");
            var database = _Context.Database.GetDbConnection().Database;
            var schema = _Context.Employees.EntityType.GetSchema() ?? "dbo";
            var table = _Context.Employees.EntityType.GetTableName();

            _Logger.LogInformation($"Bulk insert format path: {formatFilePath}");

            var query = @$"BULK INSERT [{database}].[{schema}].[{table}] FROM '{_FilePath}' WITH (FORMATFILE = '{formatFilePath}')";

            var count = await _Context.Database.ExecuteSqlRawAsync(query, cancelToken);

            _Logger.LogInformation("Bulk insert Done!");

            return new EmployeeImportResult(_GroupId.Value, count);
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
            _Writer?.Dispose();

            try
            {
                if (_FilePath != null)
                    File.Delete(_FilePath);
            }
            catch
            {
            }
        }

    }
}
