using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace vv.Infrastructure.Serialization.JsonConverters
{
    /// <summary>
    /// A custom JSON converter for serializing and deserializing <see cref="DateOnly"/> values.
    /// Formats as "yyyy-MM-dd".
    /// </summary>
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle null values
            if (reader.TokenType == JsonTokenType.Null)
                throw new JsonException("Cannot convert null value to DateOnly.");

            var dateString = reader.GetString();

            // Handle empty strings
            if (string.IsNullOrEmpty(dateString))
                throw new JsonException("Cannot convert empty string to DateOnly.");

            // Try to parse with specific format first
            if (DateOnly.TryParseExact(dateString, Format, CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out var date))
                return date;

            // Fall back to standard parsing
            if (DateOnly.TryParse(dateString, CultureInfo.InvariantCulture,
                                 DateTimeStyles.None, out date))
                return date;

            throw new JsonException($"Unable to parse '{dateString}' as a valid DateOnly value. Expected format: '{Format}'.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}