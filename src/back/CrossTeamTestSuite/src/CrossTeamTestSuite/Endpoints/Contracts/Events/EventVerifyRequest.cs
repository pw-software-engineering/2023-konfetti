using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventVerifyRequest: IRequest
{
    [JsonIgnore] public string Path => "/event/decide";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid Id { get; set; }
    public bool IsAccepted { get; set; }
}
