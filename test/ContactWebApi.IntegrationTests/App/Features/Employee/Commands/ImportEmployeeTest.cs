using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Domain.Exceptions;
using static ContactWebApi.IntegrationTests.CommonSetupFixture;


namespace ContactWebApi.IntegrationTests.App.Features.Employee.Commands
{
    public class ImportEmployeeTest
    {
        private ContactTestDbInMemory? _Fixture;

        [SetUp]
        public void SetupTest()
        {
            var testDb = new ContactTestDbInMemory();
            testDb.CreateDatabase();

            _Fixture = testDb;
        }


        [Test]
        [TestCase(@".\Resources\Files\contact_3.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_3.json", EImportDataType.Json)]
        public async Task ImportEmployee_FromStream(string filePath, EImportDataType dataType)
        {
            using var context = _Fixture!.CreateContext();

            using var stream = File.OpenRead(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromStreamRequest(dataType, stream);
            var handler = new ImportEmployeeFromStreamRequestHandler(importer);

            var result = await handler.Handle(request, default);

            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_3.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_3.json", EImportDataType.Json)]
        public async Task ImportEmployee_FromText(string filePath, EImportDataType dataType)
        {
            using var context = _Fixture!.CreateContext();

            var text = await File.ReadAllTextAsync(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromTextRequest(dataType, text);
            var handler = new ImportEmployeeFromTextRequestHandler(importer);

            var result = await handler.Handle(request, default);

            Assert.That(result, Has.Count.EqualTo(3));
        }

        [Test]
        public void NotImplementedParserException_FromStream()
        {
            using var context = _Fixture!.CreateContext();

            var filePath = @".\Resources\Files\contact_3.csv";
            using var stream = File.OpenRead(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromStreamRequest(EImportDataType.Unknown, stream);
            var handler = new ImportEmployeeFromStreamRequestHandler(importer);

            Assert.Throws<NotImplementedException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }

        [Test]
        public void NotImplementedParserException_FromText()
        {
            using var context = _Fixture!.CreateContext();

            var filePath = @".\Resources\Files\contact_3.csv";
            var text = File.ReadAllText(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromTextRequest(EImportDataType.Unknown, text);
            var handler = new ImportEmployeeFromTextRequestHandler(importer);

            Assert.Throws<NotImplementedException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }

        [Test]
        public void DuplicatedRecordException_FromStream()
        {
            using var context = _Fixture!.CreateContext();

            var filePath = @".\Resources\Files\contact_3.csv";
            var importer = CreateEmployeeImporterDefault(context);

            using (var stream = File.OpenRead(filePath))
            {
                var request = new ImportEmployeeFromStreamRequest(EImportDataType.Csv, stream);
                var handler = new ImportEmployeeFromStreamRequestHandler(importer);

                handler.Handle(request, default).GetAwaiter().GetResult();
            }

            using (var stream = File.OpenRead(filePath))
            {
                var request = new ImportEmployeeFromStreamRequest(EImportDataType.Csv, stream);
                var handler = new ImportEmployeeFromStreamRequestHandler(importer);

                Assert.Throws<DuplicatedRecordException>(() =>
                {
                    handler.Handle(request, default).GetAwaiter().GetResult();
                });
            }
        }

        [Test]
        public void DuplicatedRecordException_FromText()
        {
            using var context = _Fixture!.CreateContext();

            var filePath = @".\Resources\Files\contact_3.json";
            var text = File.ReadAllText(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromTextRequest(EImportDataType.Json, text);
            var handler = new ImportEmployeeFromTextRequestHandler(importer);
            handler.Handle(request, default).GetAwaiter().GetResult();

            request = new ImportEmployeeFromTextRequest(EImportDataType.Json, text);
            handler = new ImportEmployeeFromTextRequestHandler(importer);

            Assert.Throws<DuplicatedRecordException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_invalid_format1.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_invalid_format1.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_overlen.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_name_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_name_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_name_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_name_overlen.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_overlen.json", EImportDataType.Json)]

        public void InvalidModelException_FromStream(string filePath, EImportDataType dataType)
        {
            using var context = _Fixture!.CreateContext();

            using var stream = File.OpenRead(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromStreamRequest(dataType, stream);
            var handler = new ImportEmployeeFromStreamRequestHandler(importer);

            Assert.Throws<InvalidModelException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_invalid_format2.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_invalid_format2.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_email_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_email_overlen.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_joined_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_name_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_name_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_name_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_name_overlen.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_empty.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_empty.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_invalid.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_invalid.json", EImportDataType.Json)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_overlen.csv", EImportDataType.Csv)]
        [TestCase(@".\Resources\Files\contact_wrong_tel_overlen.json", EImportDataType.Json)]
        public void InvalidModelException_FromText(string filePath, EImportDataType dataType)
        {
            using var context = _Fixture!.CreateContext();

            var text = File.ReadAllText(filePath);
            var importer = CreateEmployeeImporterDefault(context);

            var request = new ImportEmployeeFromTextRequest(dataType, text);
            var handler = new ImportEmployeeFromTextRequestHandler(importer);

            Assert.Throws<InvalidModelException>(() =>
            {
                handler.Handle(request, default).GetAwaiter().GetResult();
            });
        }


    }
}
