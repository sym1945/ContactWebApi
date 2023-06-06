using ContactWebApi.Infra.Datas.Contact;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact
{
    public class ContactTestDbMssql : IContactTestDb
    {
        private const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=ContactDbTest";

        public ContactTestDbMssql()
        {
        }

        public void CreateDatabase()
        {
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
        }

        public ContactDbContext CreateContext()
        {
            return new ContactDbContext(
                new DbContextOptionsBuilder<ContactDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
        }
    }
}
