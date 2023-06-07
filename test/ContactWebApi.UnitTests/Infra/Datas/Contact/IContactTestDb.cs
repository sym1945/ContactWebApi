using ContactWebApi.Infra.Datas.Contact;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact
{
    public interface IContactTestDb
    {
        void CreateDatabase();

        void DeleteDatabase();

        ContactDbContext CreateContext();
    }
}
