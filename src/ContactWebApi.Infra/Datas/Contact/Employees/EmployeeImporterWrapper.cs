using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Infra.Constants;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;


namespace ContactWebApi.Infra.Datas.Contact.Employees
{
    public class EmployeeImporterWrapper : IEmployeeImporter
    {
        private readonly IEmployeeImporter _Importer;

        public EmployeeImporterWrapper(IEmployeeImporter importer)
        {
            _Importer = importer;
        }

        public Task AddAsync(EmployeeDto employee, CancellationToken cancelToken = default)
        {
            return _Importer.AddAsync(employee, cancelToken);
        }

        public async Task<EmployeeImportResult> SaveAsync(CancellationToken cancelToken = default)
        {
            try
            {
                var result = await _Importer.SaveAsync(cancelToken);
                return result;
            }
            catch (SqlException se) when (se.Number == MssqlErrorCode.CannotInsertDuplicatedWithUniqueIndex)
            {
                throw CreateException();
            }
            catch (SqliteException se) when (se.SqliteExtendedErrorCode == SqliteErrorCode.SQLITE_CONSTRAINT_UNIQUE)
            {
                throw CreateException();
            }
            catch (Exception ex) when (ex.InnerException is SqlException se && se.Number == MssqlErrorCode.CannotInsertDuplicatedWithUniqueIndex)
            {
                throw CreateException();
            }
            catch (Exception ex) when (ex.InnerException is SqliteException se && se.SqliteExtendedErrorCode == SqliteErrorCode.SQLITE_CONSTRAINT_UNIQUE)
            {
                throw CreateException();
            }
        }

        public ValueTask DisposeAsync()
        {
            return _Importer.DisposeAsync();
        }

        private static DuplicatedRecordException CreateException()
        {
            return new DuplicatedRecordException("Email or Tel number is already registered.");
        }
    }
}
