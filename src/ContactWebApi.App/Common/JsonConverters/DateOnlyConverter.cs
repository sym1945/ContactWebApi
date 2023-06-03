﻿using System.Text.Json.Serialization;
using System.Text.Json;

namespace ContactWebApi.App.Common.JsonConverters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _SerializationFormat;

        public DateOnlyConverter() : this(null)
        {
        }

        public DateOnlyConverter(string? serializationFormat)
        {
            _SerializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_SerializationFormat));
        }
    }
}
