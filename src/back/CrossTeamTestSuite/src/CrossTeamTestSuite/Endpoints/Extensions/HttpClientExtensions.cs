using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;
using CrossTeamTestSuite.Endpoints.Converters.JsonConverters;
using FluentAssertions;

namespace CrossTeamTestSuite.Endpoints.Extensions;

public static class HttpClientExtensions
{
    private static JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new DateOnlyConverter(),
            new DateTimeConverter()
        }
    };
    
    public async static Task CallEndpointSuccessAsync<TRequest>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest
    {
        var response = await client.PostOrGetAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public async static Task<TResponse?> CallEndpointSuccessAsync<TRequest, TResponse>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
    {
        var response = await client.PostOrGetAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        return await response.Content.ReadFromJsonAsync<TResponse>(jsonSerializerOptions);
    }
    
    private static Task<HttpResponseMessage> PostOrGetAsync<TRequest>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest
    {
        if (request.Type == RequestType.Post)
        {
            return client.PostAsJsonAsync(request.Path, request, jsonSerializerOptions);
        }

        var queryParamConverter = new GetQueryParamConverter<TRequest>();
        return client.GetAsync(request.Path + queryParamConverter.GetParams(request));
    }
}
