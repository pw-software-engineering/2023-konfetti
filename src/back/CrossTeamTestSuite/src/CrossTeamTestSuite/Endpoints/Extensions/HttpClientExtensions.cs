using System.Net.Http.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Extensions;

public static class HttpClientExtensions
{
    public async static Task CallEndpointAsync<TRequest>(this HttpClient client, TRequest request)
        where TRequest : IRequest
    {
        var response = await client.PostAsJsonAsync(request.Path, request);
    }
    
    public async static Task<TResponse?> CallEndpointAsync<TRequest, TResponse>(this HttpClient client, TRequest request)
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        var response = await client.PostAsJsonAsync(request.Path, request);

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
