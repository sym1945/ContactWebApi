using ContactWebApi.App.Common.Converters;
using ContactWebApi.App.Features.Employee.DTOs;
using System.Text.Json;

namespace ContactWebApi.App.Features.Employee.Parsers
{
    public class EmployeeJsonParser : IEmployeeParser
    {
        private static JsonSerializerOptions _Option = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        static EmployeeJsonParser()
        {
            _Option.Converters.Add(new DateOnlyJsonConverter());
        }

        public IEnumerable<EmployeeDto> Parse(string text)
        {
            using var jsonDoc = JsonDocument.Parse(text);
            foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
            {
                var employee = jsonElement.Deserialize<EmployeeDto>(_Option);
                yield return employee!;
            }
        }

        public async IAsyncEnumerable<EmployeeDto> Parse(Stream stream)
        {
            await foreach (var employee in JsonSerializer.DeserializeAsyncEnumerable<EmployeeDto>(stream, _Option))
            {
                yield return employee!;
            }
        }
    }

}
