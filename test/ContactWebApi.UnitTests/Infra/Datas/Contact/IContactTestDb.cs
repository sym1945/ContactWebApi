using ContactWebApi.Infra.Datas.Contact;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact
{
    internal interface IContactTestDb
    {
        void CreateDatabase();

        void DeleteDatabase();

        ContactDbContext CreateContext();
    }
}
