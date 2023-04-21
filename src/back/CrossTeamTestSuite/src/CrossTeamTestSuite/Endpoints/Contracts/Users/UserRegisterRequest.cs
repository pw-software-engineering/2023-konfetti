using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Users;

public class UserRegisterRequest: IRequest
{
    [JsonIgnore]
    public string Path => "/user/register";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;

    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
}
