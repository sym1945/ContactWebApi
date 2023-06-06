using System.Text.Json.Serialization;
using System.Text.Json;

namespace ContactWebApi.App.Common.Converters
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            return DateOnly.TryParse(str, out DateOnly result) ? result : null;
        }

        public override DateOnly? ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            return DateOnly.TryParse(str, out DateOnly result) ? result : null;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            var isoDate = value?.ToString("O") ?? string.Empty;
            writer.WriteStringValue(isoDate);
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            var isoDate = value?.ToString("O") ?? string.Empty;
            writer.WritePropertyName(isoDate);
        }
    }
}
