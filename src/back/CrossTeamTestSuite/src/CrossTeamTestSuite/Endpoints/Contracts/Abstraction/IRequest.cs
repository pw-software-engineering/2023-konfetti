using System.Text.Json.Serialization;

namespace CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

public interface IRequest<TResponse> : IRequest
    where TResponse : class
{
    [JsonIgnore]
    public new string Path { get; }
    [JsonIgnore]
    public new RequestType Type { get; }
}

public interface IRequest
{
    [JsonIgnore]
    public string Path { get; }
    [JsonIgnore]
    public RequestType Type { get; }
}
