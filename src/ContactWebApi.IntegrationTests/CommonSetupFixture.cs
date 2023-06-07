using AutoMapper;
using ContactWebApi.App.Common.Extensions;
using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.Domain.Entities;
using ContactWebApi.Infra.Datas.Contact;
using ContactWebApi.Infra.Datas.Contact.Employees;
using ContactWebApi.UnitTests;

namespace ContactWebApi.IntegrationTests
{
    [SetUpFixture]
    public class CommonSetupFixture
    {
        private static readonly IMapper _Mapper;

        static CommonSetupFixture()
        {
            _Mapper = Common.GetMapper();
        }


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        public static IMapper GetMapper()
        {
            return _Mapper;
        }

        public static IEmployeeImporter CreateEmployeeImporterDefault(ContactDbContext context)
        {
            var mapper = GetMapper();

            var importer = new EmployeeImporterWrapper(
                new EmployeeImporterDefault(context, mapper)
            );

            return importer;
        }

        public static async Task<IList<Employee>> AddMockDatas(ContactDbContext context, int itemCount = 3)
        {
            var maxItemCount = 9999_9999;
            if (itemCount > maxItemCount)
                itemCount = maxItemCount;

            var mockDatas = new List<Employee>(itemCount);

            for (int i = 1; i <= itemCount; ++i)
            {
                var name = $"{i:D8}";
                var email = $"{name}@gmail.com";
                var tel = $"010{name}".FormatTel();
                var joined = new DateTime(2023, 6, 6);

                mockDatas.Add(new Employee { Name = name, Email = email, Tel = tel, Joined = joined });
            }

            var group = new EmployeeGroup
            {
                Employees = mockDatas
            };

            await context.EmployeeGroups.AddAsync(group);
            await context.SaveChangesAsync();

            return mockDatas;
        }

    }
}
