using ContactWebApi.Infra.Datas.Contact;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact
{
    public class ContactTestDb : IContactTestDb
    {
        private readonly IContactTestDb _TestDb;

        public ContactTestDb(EDbProvider dbProvider)
        {
            _TestDb = dbProvider switch
            {
                EDbProvider.Mssql => new ContactTestDbMssql(),
                _ => new ContactTestDbSqlite(),
            };
        }

        public void CreateDatabase()
        {
            _TestDb.CreateDatabase();
        }

        public void DeleteDatabase()
        {
            _TestDb.DeleteDatabase();
        }

        public ContactDbContext CreateContext()
        {
            return _TestDb.CreateContext();
        }
    }

    public enum EDbProvider
    {
        Mssql,
        Sqllite
    }
}
