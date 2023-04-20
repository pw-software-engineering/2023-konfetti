using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Users;

public class UserViewRequest: IRequest<UserDto>
{
    [JsonIgnore]
    public string Path => "/user/view";
    [JsonIgnore]
    public RequestType Type => RequestType.Get;
}
