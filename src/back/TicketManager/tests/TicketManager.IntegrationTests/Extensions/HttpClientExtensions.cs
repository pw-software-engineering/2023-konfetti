using System.Net;
using FastEndpoints;
using FluentAssertions;

namespace TicketManager.IntegrationTests.Extensions;

public static class HttpClientExtensions
{
    public async static Task<TResponse> PostSuccessAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
        where TEndpoint : Endpoint<TRequest, TResponse>
        where TRequest : class
    {
        var response = await client.POSTAsync<TEndpoint, TRequest, TResponse>(request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        return response.Result!;
    }
    
    public async static Task PostSuccessAsync<TEndpoint, TRequest>(this HttpClient client, TRequest request)
        where TEndpoint : Endpoint<TRequest>
        where TRequest : class
    {
        var response = await client.POSTAsync<TEndpoint, TRequest>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    public async static Task<TResponse> GetSuccessAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
        where TEndpoint : Endpoint<TRequest, TResponse>
        where TRequest : class
    {
        var response = await client.GETAsync<TEndpoint, TRequest, TResponse>(request);

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        return response.Result!;
    }
}
