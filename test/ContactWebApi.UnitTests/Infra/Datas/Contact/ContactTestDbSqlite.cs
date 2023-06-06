using ContactWebApi.Infra.Datas.Contact;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact
{
    public class ContactTestDbSqlite : IContactTestDb
    {
        private readonly string ConnectionString = @"DataSource=ContactDb;mode=memory;cache=shared";
        private SqliteConnection? _KeepAliveConnection;

        public ContactTestDbSqlite()
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
                    .UseSqlite(ConnectionString)
                    .Options);
        }
    }
}
