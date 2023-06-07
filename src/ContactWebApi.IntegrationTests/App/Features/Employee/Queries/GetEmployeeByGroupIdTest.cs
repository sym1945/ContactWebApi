using AutoMapper;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Infra.Datas.Contact;
using Microsoft.EntityFrameworkCore;
using static ContactWebApi.IntegrationTests.CommonSetupFixture;


namespace ContactWebApi.IntegrationTests.App.Features.Employee.Queries
{
    public class GetEmployeeByGroupIdTest
    {
        private ContactDbContext? _Context;
        private IMapper _Mapper;
        private int _ItemCount = 3;

        [SetUp]
        public async Task SetupTest()
        {
            var testDb = new ContactTestDbInMemory();
            testDb.CreateDatabase();

            _Context = testDb.CreateContext();
            _Mapper = GetMapper();

            await AddMockDatas(_Context, _ItemCount);
        }

        [Test]
        public async Task GetByGroupId()
        {
            var request = new GetEmployeeByGroupIdRequest { GroupId = 1 };
            var handler = new GetEmployeeByGroupIdRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default).ToArrayAsync();

            Assert.That(result.Count, Is.EqualTo(_ItemCount));
        }

        [Test]
        public async Task NotExistGropuId()
        {
            var request = new GetEmployeeByGroupIdRequest { GroupId = int.MaxValue };
            var handler = new GetEmployeeByGroupIdRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default).ToArrayAsync();

            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
