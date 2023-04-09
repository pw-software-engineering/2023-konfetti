using CrossTeamTestSuite.Contracts.Abstraction;

namespace CrossTeamTestSuite.Contracts.Accounts;

public class AccountLoginRequest : IRequest<AccountLoginResponse>
{
    public string Path => "/account/login";
    public RequestType Type => RequestType.Post;
}
