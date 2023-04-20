using System.Net.Http.Json;
using System.Text.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;
using CrossTeamTestSuite.Endpoints.Converters.JsonConverters;

namespace CrossTeamTestSuite.Endpoints.Extensions;

public static class HttpClientExtensions
{
    private static JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        IgnoreNullValues = true,
        Converters =
        {
            new DateOnlyConverter(),
        }
    };
    
    public async static Task CallEndpointAsync<TRequest>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest
    {
        var response = await client.PostOrGetAsync(request);
    }

    public async static Task<TResponse?> CallEndpointAsync<TRequest, TResponse>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
    {
        var response = await client.PostOrGetAsync(request);

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
