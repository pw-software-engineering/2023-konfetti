using System.Globalization;
using System.Text.Json;
using FluentAssertions;
using TicketManager.Core.Services.Extensions.JsonConverters;
using Xunit;

namespace TicketManager.Core.ServicesTests.Extensions.JsonConverters;

public class DateOnlyConverterTests
{
    private class Dto
    {
        public DateOnly Date { get; set; }
    }

    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2000, 12, 31)]
    [InlineData(1, 1, 1)]
    [InlineData(3035, 2, 27)]
    public void WhenConverterIsNotProvided_DeserializationShouldThrowException(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);
        var json = GetJson(date);
        
        var func = () => JsonSerializer.Deserialize<Dto>(json);
        
        func.Should().Throw<Exception>();
    }
    
    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2000, 12, 31)]
    [InlineData(1, 1, 1)]
    [InlineData(3035, 2, 27)]
    public void WhenConverterIsProvided_DeserializationShouldReturnValue(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);
        var json = GetJson(date);
        
        var result = JsonSerializer.Deserialize<Dto>(json, new JsonSerializerOptions { Converters = { new DateOnlyConverter() } });

        result!.Date.Should().Be(date);
    }

    private string GetJson(DateOnly date)
    {
        return "{ \"Date\": \"" + date + "\" }";
    }
}
