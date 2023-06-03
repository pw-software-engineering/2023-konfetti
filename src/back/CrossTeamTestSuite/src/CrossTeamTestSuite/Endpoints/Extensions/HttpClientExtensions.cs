using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Converters.GetQueryParamsConverters;
using CrossTeamTestSuite.Endpoints.Converters.JsonConverters;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        
        await ValidateStatusCodeAsync(response);
    }

    public async static Task<TResponse?> CallEndpointSuccessAsync<TRequest, TResponse>(this HttpClient client, TRequest request)
        where TRequest : class, IRequest<TResponse>
        where TResponse : class
    {
        var response = await client.PostOrGetAsync(request);

        await ValidateStatusCodeAsync(response);
        
        return await response.Content.ReadFromJsonAsync<TResponse>(jsonSerializerOptions);
    }

    private async static Task ValidateStatusCodeAsync(HttpResponseMessage response)
    {
        try
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            throw new CustomException(GetFormattedString(await response.Content.ReadAsStringAsync()), e);
        }
    }

    private static string GetFormattedString(string str)
    {
        try
        {
            return JToken.Parse(str).ToString(Formatting.Indented);
        }
        catch (Exception)
        {
            return str;
        }
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

public class CustomException : Exception
{
    public override string Message => $"{base.Message}\n\n{InnerException?.Message}";
    
    public CustomException(string message, Exception? innerException) : base(message, innerException)
    { }
}
