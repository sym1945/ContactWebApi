using AutoMapper;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Infra.Datas.Contact;
using static ContactWebApi.IntegrationTests.CommonSetupFixture;


namespace ContactWebApi.IntegrationTests.App.Features.Employee.Queries
{
    public class GetEmployeeByNameTest
    {
        private ContactDbContext? _Context;
        private IMapper _Mapper;

        [SetUp]
        public async Task SetupTest()
        {
            var testDb = new ContactTestDbInMemory();
            testDb.CreateDatabase();

            _Context = testDb.CreateContext();
            _Mapper = GetMapper();

            await AddMockDatas(_Context);
        }

        [Test]
        public async Task GetByName()
        {
            var request = new GetEmployeeByNameRequest { EmployeeName = "00000002" };
            var handler = new GetEmployeeByNameRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default).ToListAsync();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Name, Is.EqualTo("00000002"));
                Assert.That(result[0].Email, Is.EqualTo("00000002@gmail.com"));
                Assert.That(result[0].Tel, Is.EqualTo("010-0000-0002"));
                Assert.That(result[0].Joined, Is.EqualTo(new DateOnly(2023, 6, 6)));
            });
        }

        [Test]
        public async Task NotExistName()
        {
            var request = new GetEmployeeByNameRequest { EmployeeName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaacharles" };
            var handler = new GetEmployeeByNameRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default).ToListAsync();

            Assert.That(result, Has.Count.EqualTo(0));
        }
    }
}
