using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
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
            .Where(a => a.Email == req.Email && !a.IsBanned)
            .FirstOrDefaultAsync(ct);
        if (account is not null && passwordManager.DoPasswordsMatch(account.PasswordHash, req.Password))
        {
            if (account.Role == AccountRoles.Organizer)
            {
                var organizer = await coreDbContext.Organizers.Where(a => a.Id == account.Id).FirstOrDefaultAsync(ct);

                if (!organizer?.IsVerified() ?? false)
                {
                    await SendUnauthorizedAsync(ct);
                    return;
                }
            }
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
