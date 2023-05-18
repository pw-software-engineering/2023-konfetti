using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventPublishRequest: IRequest
{
    [JsonIgnore]
    public string Path => "/event/publish";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid Id { get; set; }
}
