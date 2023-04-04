using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicketManager.Core.Services.Extensions.JsonConverters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var temp = DateTime.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
        return new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, temp.Second, DateTimeKind.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
