using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventSaleStartRequest: IRequest
{
    [JsonIgnore]
    public string Path => "/event/sale/start";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid EventId { get; set; }
}
