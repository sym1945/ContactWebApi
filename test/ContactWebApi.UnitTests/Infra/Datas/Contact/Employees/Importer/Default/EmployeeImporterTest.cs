using AutoMapper;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Infra.Datas.Contact.Employees;
using static ContactWebApi.UnitTests.Infra.Datas.Contact.Employees.Importer.Default.DatabaseSetupFixture;
using static ContactWebApi.UnitTests.Common;

namespace ContactWebApi.UnitTests.Infra.Datas.Contact.Employees.Importer.Default
{
    [TestFixture]
    public class EmployeeImporterTest
    {
        
        private readonly static IMapper _Mapper = GetMapper();

        [Test, Order(1)]
        public async Task ImportEmployeeDefault_One()
        {
            using var context = CreateContext();

            var importer = new EmployeeImporterDefault(context, _Mapper);

            await importer.AddAsync(CreateEmployee(name: "a", email: "a@a.com", tel: "010-1111-1111"));

            var result = await importer.SaveAsync();

            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test, Order(2)]
        public async Task ImportEmployeeDefault_Multi()
        {
            using var context = CreateContext();

            var importer = new EmployeeImporterDefault(context, _Mapper);

            await importer.AddAsync(CreateEmployee(name: "b", email: "b@a.com", tel: "010-1111-1112"));
            await importer.AddAsync(CreateEmployee(name: "c", email: "c@a.com", tel: "010-1111-1113"));
            await importer.AddAsync(CreateEmployee(name: "d", email: "d@a.com", tel: "010-1111-1114"));

            var result = await importer.SaveAsync();

            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test, Order(3)]
        public async Task DuplicatedRecordException_Email()
        {
            using var context = CreateContext();

            var importer = new EmployeeImporterWrapper(
                new EmployeeImporterDefault(context, _Mapper)
            );

            await importer.AddAsync(CreateEmployee(name: "f", email: "d@a.com", tel: "010-1111-1115"));

            Assert.Throws<DuplicatedRecordException>(() =>
            {
                importer.SaveAsync().GetAwaiter().GetResult();
            });
        }

        [Test, Order(4)]
        public async Task DuplicatedRecordException_Tel()
        {
            using var context = CreateContext();

            var importer = new EmployeeImporterWrapper(
                new EmployeeImporterDefault(context, _Mapper)
            );

            await importer.AddAsync(CreateEmployee(name: "f", email: "f@a.com", tel: "010-1111-1114"));

            Assert.Throws<DuplicatedRecordException>(() =>
            {
                importer.SaveAsync().GetAwaiter().GetResult();
            });
        }


    }
}
