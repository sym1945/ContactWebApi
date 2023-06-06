using ContactWebApi.Infra.Datas.Contact;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact.Employees.Importer.MssqlBulk
{
    [SetUpFixture]
    public class DatabaseSetupFixture
    {
        private static readonly IContactTestDb _TestDb;

        static DatabaseSetupFixture()
        {
            _TestDb = new ContactTestDb(EDbProvider.Mssql);
        }


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _TestDb.CreateDatabase();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _TestDb.DeleteDatabase();
        }

        public static ContactDbContext CreateContext()
        {
            return _TestDb.CreateContext();
        }
    }
}
