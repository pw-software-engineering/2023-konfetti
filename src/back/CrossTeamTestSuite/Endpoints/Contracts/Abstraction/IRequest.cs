using System.Text.Json.Serialization;

namespace CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

public interface IRequest<TResponse> : IRequest
    where TResponse : class
{
    [JsonIgnore]
    public string Path { get; }
    [JsonIgnore]
    public RequestType Type { get; }
}

public interface IRequest
{
    [JsonIgnore]
    public string Path { get; }
    [JsonIgnore]
    public RequestType Type { get; }
}
