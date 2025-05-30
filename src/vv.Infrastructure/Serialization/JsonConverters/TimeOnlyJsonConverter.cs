using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace vv.Infrastructure.Serialization.JsonConverters
{
    /// <summary>
    /// A custom JSON converter for serializing and deserializing <see cref="TimeOnly"/> values.
    /// Formats as "HH:mm:ss".
    /// </summary>
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm:ss";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle null values
            if (reader.TokenType == JsonTokenType.Null)
                throw new JsonException("Cannot convert null value to TimeOnly.");

            var timeString = reader.GetString();

            // Handle empty strings
            if (string.IsNullOrEmpty(timeString))
                throw new JsonException("Cannot convert empty string to TimeOnly.");

            // Try to parse with specific format first
            if (TimeOnly.TryParseExact(timeString, Format, CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out var time))
                return time;

            // Fall back to standard parsing
            if (TimeOnly.TryParse(timeString, CultureInfo.InvariantCulture,
                                 DateTimeStyles.None, out time))
                return time;

            throw new JsonException($"Unable to parse '{timeString}' as a valid TimeOnly value. Expected format: '{Format}'.");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}