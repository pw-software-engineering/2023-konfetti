using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Contracts.Common;

namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventListRequest: IRequest<PaginatedResponse<EventDto>>, IPaginatedRequest
{
    [JsonIgnore]
    public string Path => "/event/list";
    [JsonIgnore]
    public RequestType Type => RequestType.Get;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
