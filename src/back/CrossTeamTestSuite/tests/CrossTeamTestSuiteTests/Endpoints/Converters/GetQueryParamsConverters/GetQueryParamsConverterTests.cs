using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;
using FluentAssertions;
using Xunit;

namespace CrossTeamTestSuiteTests.Endpoints.Converters.GetQueryParamsConverters;

public class GetQueryParamsConverterTests
{
    private class EmptyRequest : IRequest
    {
        [JsonIgnore]
        public string Path => "/path";
        [JsonIgnore]
        public RequestType Type => RequestType.Get;
    }
    
    private class Request : IRequest
    {
        [JsonIgnore]
        public string Path => "/path";
        [JsonIgnore]
        public RequestType Type => RequestType.Get;

        public int PageSize => 1;
        public int PageNumber => 12;
        public string StrValue => "str";
        public string StrValueWithSpace => "str str";
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

    [Fact]
    public void WhenRequestWithManyFieldsIsProvided_ItShouldReturnCorrectQueryParams()
    {
        var expected = "?PageSize=1&PageNumber=12&StrValue=str&StrValueWithSpace=str%20str";
        var converter = new GetQueryParamConverter<Request>();
        var request = new Request();

        var actual = converter.GetParams(request);

        actual.Should().Be(expected);
    }
}
