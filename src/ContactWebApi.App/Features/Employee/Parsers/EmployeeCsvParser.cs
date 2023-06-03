using ContactWebApi.App.Common.Extensions;
using ContactWebApi.App.Features.Employee.DTOs;
using Sylvan.Data.Csv;

namespace ContactWebApi.App.Features.Employee.Parsers
{
    public class EmployeeCsvParser : IEmployeeParser
    {
        private static CsvDataReaderOptions _Option = new CsvDataReaderOptions
        {
            HasHeaders = false,
            Delimiter = ',',
            StringFactory = TrimString
        };

        public IEnumerable<EmployeeDto> Parse(string text)
        {
            using var reader = new StringReader(text);
            using var csv = CsvDataReader.Create(reader, _Option);

            while (csv.Read())
            {
                var employee = ToRecord(csv);

                yield return employee;
            }
        }

        public async IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = CsvDataReader.Create(reader, _Option);

            var result = new List<EmployeeDto>();

            while (await csv.ReadAsync())
            {
                var employee = ToRecord(csv);

                yield return employee;
            }
        }

        private static EmployeeDto ToRecord(CsvDataReader csv)
        {
            var record = new EmployeeDto
            {
                Name = csv.GetString(0),
                Email = csv.GetString(1),
                Tel = csv.GetString(2).FormatTel(),
                Joined = csv.GetDate(3),
            };

            return record;
        }


        private static string TrimString(char[] buf, int offset, int length)
        {
            var whiteSpace = 0x20;
            int startIndex = -1;
            int endOffset = offset + length;
            int endIndex = endOffset;

            for (int i = offset; i <= endOffset; ++i)
            {
                if (buf[i] != whiteSpace)
                {
                    if (startIndex == -1)
                        startIndex = i;

                    endIndex = i;
                }
            }

            if (startIndex == -1)
                startIndex = offset;

            var result = new string(buf, startIndex, endIndex - startIndex);
            return result;
        }

    }
}
