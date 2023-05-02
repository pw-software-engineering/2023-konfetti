using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Organizers;

public class OrganizerRegisterRequest: IRequest
{
    [JsonIgnore]
    public string Path => "/organizer/register";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public string TaxId { get; set; }
    public TaxIdTypeDto TaxIdType { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
}
