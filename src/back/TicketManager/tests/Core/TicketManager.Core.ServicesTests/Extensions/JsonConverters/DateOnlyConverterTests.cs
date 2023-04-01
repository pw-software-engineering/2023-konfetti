using System.Text.Json;
using System.Text.RegularExpressions;
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

    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2000, 12, 31)]
    [InlineData(1, 1, 1)]
    [InlineData(3035, 2, 27)]
    public void WhenConverterIsNotProvider_SerializationShouldThrowException(int year, int month, int day)
    {
        var dto = new Dto()
        {
            Date = new DateOnly(year, month, day),
        };

        var func = () => JsonSerializer.Serialize(dto);
        
        func.Should().Throw<Exception>();
    }
    
    [Theory]
    [InlineData(2023, 1, 1)]
    [InlineData(2000, 12, 31)]
    [InlineData(1, 1, 1)]
    [InlineData(3035, 2, 27)]
    public void WhenConverterIsProvider_SerializationShouldReturnValue(int year, int month, int day)
    {
        var dto = new Dto()
        {
            Date = new DateOnly(year, month, day),
        };
        var expected = GetJson(dto.Date);

        var actual = JsonSerializer.Serialize(dto, new JsonSerializerOptions { Converters = { new DateOnlyConverter() } });
        
        var actualWithoutWhiteSpaces = Regex.Replace(actual, "\\s", "");
        var expectedWithoutWhiteSpaces = Regex.Replace(expected, "\\s", "");

        actualWithoutWhiteSpaces.Should().BeEquivalentTo(expectedWithoutWhiteSpaces);
    }

    private string GetJson(DateOnly date)
    {
        return "{ \"Date\": \"" + date + "\" }";
    }
}
