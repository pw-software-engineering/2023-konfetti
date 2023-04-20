using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Organizers;

public class OrganizerViewRequest: IRequest<OrganizerDto>
{
    [JsonIgnore]
    public string Path => "/organizer/view";
    [JsonIgnore]
    public RequestType Type => RequestType.Get;
}
