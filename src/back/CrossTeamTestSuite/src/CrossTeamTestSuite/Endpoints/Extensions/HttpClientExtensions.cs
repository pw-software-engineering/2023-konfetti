using System.Net.Http.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;

namespace CrossTeamTestSuite.Endpoints.Extensions;

public static class HttpClientExtensions
{
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

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    
    private static Task<HttpResponseMessage> PostOrGetAsync<TRequest>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest
    {
        if (request.Type == RequestType.Post)
        {
            return client.PostAsJsonAsync(request.Path, request);
        }

        var queryParamConverter = new GetQueryParamConverter<TRequest>();
        return client.GetAsync(request.Path + queryParamConverter.GetParams(request));
    }
}
