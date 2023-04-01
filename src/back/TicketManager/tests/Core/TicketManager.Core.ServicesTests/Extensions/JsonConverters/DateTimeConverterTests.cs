using System.Text.Json;
using FluentAssertions;
using TicketManager.Core.Services.Extensions.JsonConverters;
using Xunit;

namespace TicketManager.Core.ServicesTests.Extensions.JsonConverters;

public class DateTimeConverterTests
{
    private class Dto
    {
        public DateTime Date { get; set; }
    }
    
    [Theory]
    [InlineData(2020, 1, 1, 0, 0, 0)]
    [InlineData(1000, 12, 31, 12, 59, 59)]
    [InlineData(335, 2, 28, 23, 45, 32)]
    [InlineData(2032, 11, 30, 8, 15, 0)]
    public void WhenUtcDateTimeIsProvided_ItShouldReturnUtcDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var dto = new Dto() { Date = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc) };
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });
        var actual = JsonSerializer.Deserialize<Dto>(json, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });

        actual!.Date.Should().Be(dto.Date);
        actual.Date.Kind.Should().Be(DateTimeKind.Utc);
    }
    
    [Theory]
    [InlineData(2020, 1, 1, 0, 0, 0)]
    [InlineData(1000, 12, 31, 12, 59, 59)]
    [InlineData(335, 2, 28, 23, 45, 32)]
    [InlineData(2032, 11, 30, 8, 15, 0)]
    public void WhenLocalDateTimeIsProvided_ItShouldReturnUtcDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var dto = new Dto() { Date = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Local) };
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });
        var actual = JsonSerializer.Deserialize<Dto>(json, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });

        actual!.Date.Should().Be(dto.Date);
        actual.Date.Kind.Should().Be(DateTimeKind.Utc);
    }
    
    [Theory]
    [InlineData(2020, 1, 1, 0, 0, 0)]
    [InlineData(1000, 12, 31, 12, 59, 59)]
    [InlineData(335, 2, 28, 23, 45, 32)]
    [InlineData(2032, 11, 30, 8, 15, 0)]
    public void WhenUnspecifiedDateTimeIsProvided_ItShouldReturnUtcDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        var dto = new Dto() { Date = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified) };
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });
        var actual = JsonSerializer.Deserialize<Dto>(json, new JsonSerializerOptions { Converters = { new DateTimeConverter() } });

        actual!.Date.Should().Be(dto.Date);
        actual.Date.Kind.Should().Be(DateTimeKind.Utc);
    }
}
