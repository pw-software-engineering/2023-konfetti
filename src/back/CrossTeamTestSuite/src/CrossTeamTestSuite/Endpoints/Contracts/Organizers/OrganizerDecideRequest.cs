using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Organizers;

public class OrganizerDecideRequest: IRequest
{
    [JsonIgnore]
    public string Path => "/organizer/decide";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    
    public Guid OrganizerId { get; set; }
    public bool IsAccepted { get; set; }
}
