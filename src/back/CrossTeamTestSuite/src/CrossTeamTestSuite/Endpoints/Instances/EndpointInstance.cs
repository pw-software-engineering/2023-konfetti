using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Instances;

public abstract class EndpointInstance<TRequest, TResponse> 
    where TRequest: IRequest<TResponse> 
    where TResponse: class
{
    protected HttpClient HttpClient { get; }
    protected string Token { get; set; }

    protected EndpointInstance()
    {
        HttpClient = ApiClientSingleton.GetInstance();
        Token = string.Empty;
        Configure();
    }
    
    protected void Configure()
    {
        if(Token == string.Empty)
            return;
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
    }
    
    public void SetToken(string token)
    {
        Token = token;
        Configure();
    }

    public void SetToken(CommonTokenType tokenType)
    {
        Token = CommonTokenSingleton.GetToken(tokenType);
        Configure();
    }

    public abstract Task<TResponse?> HandleEndpointAsync(TRequest request);
}

public abstract class EndpointInstance<TRequest> where TRequest: IRequest
{
    protected HttpClient HttpClient { get; }
    protected string Token { get; set; }

    protected EndpointInstance()
    {
        HttpClient = ApiClientSingleton.GetInstance();
        Token = string.Empty;
        Configure();
    }

    protected void Configure()
    {
        if(Token == string.Empty)
            return;
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
    }
    
    public void SetToken(string token)
    {
        Token = token;
        Configure();
    }

    public void SetToken(CommonTokenType tokenType)
    {
        Token = CommonTokenSingleton.GetToken(tokenType);
        Configure();
    }
    
    public abstract Task HandleEndpointAsync(TRequest request);
}
