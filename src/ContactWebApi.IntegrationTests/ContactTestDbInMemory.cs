using ContactWebApi.Infra.Datas.Contact;
using ContactWebApi.UnitTests.Infra.Datas.Contact;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.IntegrationTests
{
    public class ContactTestDbInMemory : IContactTestDb
    {
        private readonly string ConnectionString = @"DataSource=:memory:";
        private SqliteConnection? _KeepAliveConnection;

        public ContactTestDbInMemory()
        {
        }

        public void CreateDatabase()
        {
            var keepAliveConnection = new SqliteConnection(ConnectionString);
            keepAliveConnection.Open();
            _KeepAliveConnection = keepAliveConnection;

            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public void DeleteDatabase()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
            }

            _KeepAliveConnection?.Dispose();
        }

        public ContactDbContext CreateContext()
        {
            return new ContactDbContext(
                new DbContextOptionsBuilder<ContactDbContext>()
                    .UseSqlite(_KeepAliveConnection!)
                    .Options);
        }
    }
}
