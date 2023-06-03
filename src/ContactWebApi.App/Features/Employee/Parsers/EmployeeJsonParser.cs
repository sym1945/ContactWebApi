using ContactWebApi.App.Common.JsonConverters;
using ContactWebApi.App.Models;
using System.Text.Json;

namespace ContactWebApi.App.Parsers
{
    public class EmployeeJsonParser : IEmployeeParser
    {
        private static JsonSerializerOptions _Option = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        static EmployeeJsonParser()
        {
            _Option.Converters.Add(new DateOnlyConverter());
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
