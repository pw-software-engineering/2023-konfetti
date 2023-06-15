using FastEndpoints;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountUnBanEndpoint : Endpoint<AccountBanRequest>
{
    private readonly Repository<Account, Guid> accounts;

    public AccountUnBanEndpoint(Repository<Account, Guid> accounts)
    {
        this.accounts = accounts;
    }

    public override void Configure()
    {
        Post("/account/unban");
        Roles(AccountRoles.Admin);
    }

    public override async Task HandleAsync(AccountBanRequest req, CancellationToken ct)
    {
        var account = await accounts.FindAndEnsureExistenceAsync(req.AccountId, ct);
        account.UnBan();
        await accounts.UpdateAsync(account, ct);
        
        await SendOkAsync(ct);
    }
}
