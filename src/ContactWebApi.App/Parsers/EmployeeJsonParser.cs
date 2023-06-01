using ContactWebApi.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        public IList<Employee> Parse(string text)
        {
            var result = JsonSerializer.Deserialize<IList<Employee>>(text, _Option);

            return result ?? Array.Empty<Employee>();
        }

        public async IAsyncEnumerable<Employee> Parse(Stream stream)
        {
            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            await foreach (var employee in JsonSerializer.DeserializeAsyncEnumerable<Employee>(stream, _Option))
            {
                yield return employee!;
            }
        }
    }

    internal class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;

        public DateOnlyConverter() : this(null)
        {
        }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(serializationFormat));
        }
    }
}
