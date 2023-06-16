using FastEndpoints;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountBanEndpoint : Endpoint<AccountBanRequest>
{
    private readonly Repository<Account, Guid> accounts;

    public AccountBanEndpoint(Repository<Account, Guid> accounts)
    {
        this.accounts = accounts;
    }

    public override void Configure()
    {
        Post("/account/ban");
        Roles(AccountRoles.Admin);
    }

    public override async Task HandleAsync(AccountBanRequest req, CancellationToken ct)
    {
        var account = await accounts.FindAndEnsureExistenceAsync(req.AccountId, ct);
        account.Ban();
        await accounts.UpdateAsync(account, ct);
        
        await SendOkAsync(ct);
    }
}
