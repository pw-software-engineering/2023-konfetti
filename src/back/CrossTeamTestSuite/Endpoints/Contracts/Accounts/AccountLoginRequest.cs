using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Accounts;

public class AccountLoginRequest : IRequest<AccountLoginResponse>
{
    public string Path => "/account/login";
    public RequestType Type => RequestType.Post;
}
