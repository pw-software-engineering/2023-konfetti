using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;
using FluentAssertions;
using Xunit;

namespace CrossTeamTestSuiteTests.Endpoints.Converters.GetQueryParamsConverters;

public class GetQueryParamsConverterTests
{
    private class EmptyRequest : IRequest
    {
        public string Path => "/path";
        public RequestType Type => RequestType.Get;
    }
    
    [Fact]
    public void WhenEmptyRequestIsProvided_ItShouldReturnEmptyQueryParams()
    {
        var expected = "";
        var converter = new GetQueryParamConverter<EmptyRequest>();
        var request = new EmptyRequest();

        var actual = converter.GetParams(request);

        actual.Should().Be(expected);
    }
}
