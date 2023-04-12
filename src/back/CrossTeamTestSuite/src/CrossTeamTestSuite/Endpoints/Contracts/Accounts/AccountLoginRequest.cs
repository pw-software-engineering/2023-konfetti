using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Accounts;

public class AccountLoginRequest : IRequest<AccountLoginResponse>
{
    [JsonIgnore]
    public string Path => "/account/login";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
