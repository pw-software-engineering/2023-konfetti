using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.PasswordManagers;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountLoginEndpoint: Endpoint<AccountLoginRequest, AccountLoginResponse>
{
    private readonly Repository<Account, Guid> accounts;
    private readonly CoreDbContext coreDbContext;
    private readonly PasswordManager passwordManager;

    public AccountLoginEndpoint(Repository<Account, Guid> accounts, CoreDbContext coreDbContext, PasswordManager passwordManager)
    {
        this.accounts = accounts;
        this.coreDbContext = coreDbContext;
        this.passwordManager = passwordManager;
    }

    public override void Configure()
    {
        Get("/account/login");
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
            // return token
        }

        await SendUnauthorizedAsync(ct);
    }
}
