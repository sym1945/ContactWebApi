using AutoMapper;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Infra.Datas.Contact;
using static ContactWebApi.IntegrationTests.CommonSetupFixture;


namespace ContactWebApi.IntegrationTests.App.Features.Employee.Queries
{
    public class GetEmployeePageTest
    {
        private ContactDbContext? _Context;
        private IMapper _Mapper;
        private IPageUriCreator _PageUriCreator;
        private int _ItemCount = 50;

        [SetUp]
        public async Task SetupTest()
        {
            var testDb = new ContactTestDbInMemory();
            testDb.CreateDatabase();

            _Context = testDb.CreateContext();
            _Mapper = GetMapper();
            _PageUriCreator = new TestPageUriCreator();

            await AddMockDatas(_Context, _ItemCount);
        }

        [Test]
        [TestCase(1, 10, ExpectedResult = 10)]
        [TestCase(2, 10, ExpectedResult = 10)]
        [TestCase(3, 10, ExpectedResult = 10)]
        [TestCase(4, 10, ExpectedResult = 10)]
        [TestCase(5, 10, ExpectedResult = 10)]
        [TestCase(6, 10, ExpectedResult = 0)]
        [TestCase(6, 9, ExpectedResult = 5)]
        [TestCase(7, 9, ExpectedResult = 0)]
        public async Task<int> GetPageSize(int page, int pageSize)
        {
            var totalPageCount = (int)Math.Ceiling((double)_ItemCount / pageSize);

            var request = new GetEmployeePageRequest { Page = page, PageSize = pageSize };
            var handler = new GetEmployeePageRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default);
            Assert.Multiple(() =>
            {
                Assert.That(result.Items.Count, Is.EqualTo(result.ItemCount));
                Assert.That(result.CurrentPageNo, Is.EqualTo(page));
                Assert.That(result.TotalPageCount, Is.EqualTo(totalPageCount));
            });

            return result.ItemCount;
        }

        [Test]
        [TestCase(1, 10)]
        [TestCase(2, 10)]
        [TestCase(3, 10)]
        [TestCase(4, 10)]
        [TestCase(5, 10)]
        [TestCase(6, 10)]
        [TestCase(6, 9)]
        [TestCase(7, 9)]
        public async Task CreatePageUri(int page, int pageSize)
        {
            Uri? first = null;
            Uri? prev = null;
            Uri? next = null;
            Uri? last = null;

            var totalPageCount = (int)Math.Ceiling((double)_ItemCount / pageSize);
            if (totalPageCount > 0)
            {
                if (page > 1)
                {
                    first = _PageUriCreator.CreateUri(1, pageSize);
                    prev = _PageUriCreator.CreateUri(page - 1, pageSize);
                }

                if (page < totalPageCount)
                {
                    next = _PageUriCreator.CreateUri(page + 1, pageSize);
                    last = _PageUriCreator.CreateUri(totalPageCount, pageSize);
                }
            }

            var request = new GetEmployeePageRequest { Page = page, PageSize = pageSize };
            request.SetPageUriCreator(_PageUriCreator);
            var handler = new GetEmployeePageRequestHandler(_Context!, _Mapper);

            var result = await handler.Handle(request, default);
            Assert.Multiple(() =>
            {
                Assert.That(result.First, Is.EqualTo(first));
                Assert.That(result.Prev, Is.EqualTo(prev));
                Assert.That(result.Next, Is.EqualTo(next));
                Assert.That(result.Last, Is.EqualTo(last));
            });
        }

        [Test]
        [TestCase(null, 10)]
        [TestCase(0, 10)]
        [TestCase(1, null)]
        [TestCase(1, 0)]
        [TestCase(1, 101)]
        public void InvalidModelException(int? page, int? pageSize)
        {
            var request = new GetEmployeePageRequest { Page = page, PageSize = pageSize };
            var handler = new GetEmployeePageRequestHandler(_Context!, _Mapper);

            Assert.Throws<InvalidModelException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }
    }

    public class TestPageUriCreator : IPageUriCreator
    {
        public Uri? CreateUri(int page, int pageSize)
        {
            return new Uri($"http://localhost/api/employee?page={page}&pageSize={pageSize}");
        }
    }
}
