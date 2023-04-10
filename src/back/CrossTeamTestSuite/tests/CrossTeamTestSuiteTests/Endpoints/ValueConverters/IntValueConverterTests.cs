using CrossTeamTestSuite.Endpoints.Converters.ValueConverters;
using FluentAssertions;
using Xunit;

namespace CrossTeamTestSuiteTests.Endpoints.ValueConverters;

public class IntValueConverterTests
{
    private readonly IntValueConverter converter = new();
    
    [Theory]
    [InlineData("Name", -10)]
    [InlineData("PageSize", 4)]
    [InlineData("PageNumber", 0)]
    [InlineData("NumberOfRows", 10)]
    public void WhenNumberIsProvided_ItShouldReturnQueryParam(string name, int value)
    {
        var expected = $"{name}={value}";

        var actual = converter.Convert(name, value);

        actual.Should().Be(expected);
    }
}
