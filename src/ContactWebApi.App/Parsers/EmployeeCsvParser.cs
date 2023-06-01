using ContactWebApi.App.Models;
using Sylvan.Data.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWebApi.App.Parsers
{
    public class EmployeeCsvParser : IEmployeeParser
    {
        private static CsvDataReaderOptions _Option = new CsvDataReaderOptions
        {
            HasHeaders = false,
            Delimiter = ',',
            StringFactory = TrimString
        };

        public IList<Employee> Parse(string text)
        {
            using var reader = new StringReader(text);
            using var csv = CsvDataReader.Create(reader, _Option);

            var result = new List<Employee>();

            while (csv.Read())
            {
                var contact = new Employee
                {
                    Name = csv.GetString(0),
                    Email = csv.GetString(1),
                    Tel = csv.GetString(2),
                    Joined = csv.GetDate(3),
                };

                result.Add(contact);
            }

            return result;
        }

        public async IAsyncEnumerable<Employee> Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = CsvDataReader.Create(reader, _Option);

            var result = new List<Employee>();

            while (await csv.ReadAsync())
            {
                var contact = new Employee
                {
                    Name = csv.GetString(0),
                    Email = csv.GetString(1),
                    Tel = csv.GetString(2),
                    Joined = csv.GetDate(3),
                };

                yield return contact;
            }
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
