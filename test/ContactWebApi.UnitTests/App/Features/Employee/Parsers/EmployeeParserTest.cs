using ContactWebApi.App.Features.Employee.Parsers;
using ContactWebApi.Domain.Enums;
using ContactWebApi.Domain.Exceptions;

namespace ContactWebApi.UnitTests.App.Features.Employee.Parsers
{
    public class EmployeeParserTest
    {
        [Test]
        [TestCase(@".\Resources\Files\contact_3.csv", ExpectedResult = 3)]
        [TestCase(@".\Resources\Files\contact_100_000.csv", ExpectedResult = 100_000)]
        public int ParseEmployeeFromCsvText(string filePath)
        {
            var text = File.ReadAllText(filePath);

            var parser = new EmployeeParser(EImportDataType.Csv);

            var employees = parser.Parse(text).ToList();

            return employees.Count;
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_3.csv", ExpectedResult = 3)]
        [TestCase(@".\Resources\Files\contact_100_000.csv", ExpectedResult = 100_000)]
        public async Task<int> ParseEmployeeFromCsvStream(string filePath)
        {
            using var stream = File.OpenRead(filePath);

            var parser = new EmployeeParser(EImportDataType.Csv);

            var employees = await parser.Parse(stream).ToListAsync();

            return employees.Count;
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_3.json", ExpectedResult = 3)]
        [TestCase(@".\Resources\Files\contact_100_000.json", ExpectedResult = 100_000)]
        public int ParseEmployeeFromJsonText(string filePath)
        {
            var text = File.ReadAllText(filePath);

            var parser = new EmployeeParser(EImportDataType.Json);

            var employees = parser.Parse(text).ToList();

            return employees.Count;
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_3.json", ExpectedResult = 3)]
        [TestCase(@".\Resources\Files\contact_100_000.json", ExpectedResult = 100_000)]
        public async Task<int> ParseEmployeeFromJsonStream(string filePath)
        {
            using var stream = File.OpenRead(filePath);

            var parser = new EmployeeParser(EImportDataType.Json);

            var employees = await parser.Parse(stream).ToListAsync();

            return employees.Count;
        }

        [Test]
        public void NotImplementedParserExcetion()
        {
            Assert.Throws<NotImplementedException>(() =>
            {
                _ = new EmployeeParser(EImportDataType.Unknown);
            });
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_invalid_format1.csv")]
        [TestCase(@".\Resources\Files\contact_invalid_format2.csv")]
        [TestCase(@".\Resources\Files\contact_3.json")]
        public void InvalidModelExcetionFromCsv(string filePath)
        {
            Assert.Throws<InvalidModelException>(() =>
            {
                ParseEmployeeFromCsvText(filePath);
            });

            Assert.Throws<InvalidModelException>(() =>
            {
                ParseEmployeeFromCsvStream(filePath).GetAwaiter().GetResult();
            });
        }

        [Test]
        [TestCase(@".\Resources\Files\contact_invalid_format1.json")]
        [TestCase(@".\Resources\Files\contact_invalid_format2.json")]
        [TestCase(@".\Resources\Files\contact_3.csv")]
        public void InvalidModelExcetionFromJson(string filePath)
        {
            Assert.Throws<InvalidModelException>(() =>
            {
                ParseEmployeeFromJsonText(filePath);
            });

            Assert.Throws<InvalidModelException>(() =>
            {
                ParseEmployeeFromJsonStream(filePath).GetAwaiter().GetResult();
            });
        }




    }
}
