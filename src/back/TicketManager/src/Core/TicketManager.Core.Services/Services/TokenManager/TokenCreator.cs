using FastEndpoints.Security;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.Configuration;

namespace TicketManager.Core.Services.Services.TokenManager;

public class TokenCreator
{
    private readonly TokenConfiguration tokenConfiguration;

    public TokenCreator(TokenConfiguration tokenConfiguration)
    {
        this.tokenConfiguration = tokenConfiguration;
    }

    public string GetToken(Account account)
    {
        return JWTBearer.CreateToken(
            signingKey: tokenConfiguration.signingKey,
            expireAt: DateTime.UtcNow.AddHours(4),
            claims: new[] { ("AccountId", account.Id.ToString()) },
            roles: new[] { account.Role }).ToString();
    }
}
