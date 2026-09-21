using System.Text.Json;
using System.Text.Json.Serialization;

namespace AyuLanka.AMS.AMSWeb.Converters
{
    /// <summary>
    /// Converts TimeSpan to/from JSON strings in "HH:MM:SS" or "HH:MM" format.
    /// The default System.Text.Json serializer only accepts "HH:MM:SS"; this converter
    /// also accepts the short "HH:MM" format sent by HTML time inputs.
    /// </summary>
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                return TimeSpan.Zero;

            // Accept "HH:MM" (5 chars) by appending seconds
            if (value.Length == 5 && value[2] == ':')
                value += ":00";

            if (TimeSpan.TryParse(value, out var result))
                return result;

            throw new JsonException($"Cannot convert '{value}' to TimeSpan. Expected format: HH:MM or HH:MM:SS.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            // Serialize as "HH:MM:SS" so the frontend receives a consistent format
            writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
        }
    }
}
