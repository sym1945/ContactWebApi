using AutoMapper;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Infra.Datas.Contact;
using static ContactWebApi.IntegrationTests.CommonSetupFixture;


namespace ContactWebApi.IntegrationTests.App.Features.Employee.Queries
{
    public class GetEmployeeByIdTest
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
        public async Task GetById()
        {
            var request = new GetEmployeeByIdRequest { EmployeeId = 1 };
            var handler = new GetEmployeeByIdRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default);

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo("00000001"));
                Assert.That(result.Email, Is.EqualTo("00000001@gmail.com"));
                Assert.That(result.Tel, Is.EqualTo("010-0000-0001"));
                Assert.That(result.Joined, Is.EqualTo(new DateOnly(2023, 6, 6)));
            });
        }

        [Test]
        public async Task NotExistId()
        {
            var request = new GetEmployeeByIdRequest { EmployeeId = int.MaxValue };
            var handler = new GetEmployeeByIdRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default);

            Assert.That(result, Is.Null);
        }
    }
}
