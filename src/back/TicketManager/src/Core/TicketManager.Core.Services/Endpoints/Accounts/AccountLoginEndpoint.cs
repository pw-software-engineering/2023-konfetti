using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Services.PasswordManagers;
using TicketManager.Core.Services.Services.TokenManager;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountLoginEndpoint: Endpoint<AccountLoginRequest, AccountLoginResponse>
{
    private readonly CoreDbContext coreDbContext;
    private readonly PasswordManager passwordManager;
    private readonly TokenCreator tokenCreator;
    
    public AccountLoginEndpoint(CoreDbContext coreDbContext, PasswordManager passwordManager, TokenCreator tokenCreator)
    {
        this.coreDbContext = coreDbContext;
        this.passwordManager = passwordManager;
        this.tokenCreator = tokenCreator;
    }

    public override void Configure()
    {
        Post("/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AccountLoginRequest req, CancellationToken ct)
    {
        var account = await coreDbContext
            .Accounts
            .Where(a => a.Email == req.Email)
            .FirstOrDefaultAsync(ct);
        if (account is not null && passwordManager.DoPasswordsMatch(account.PasswordHash, req.Password))
        {
            var response = new AccountLoginResponse
            {
                AccessToken = tokenCreator.GetToken(account)
            };
            await SendAsync(response, cancellation: ct);
            return;
        }

        await SendUnauthorizedAsync(ct);
    }
}
