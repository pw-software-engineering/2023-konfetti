using FastEndpoints;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.PasswordManagers;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountUpdatePasswordEndpoint: Endpoint<AccountUpdatePasswordRequest>
{
    private readonly PasswordManager passwordManager;
    private readonly Repository<Account, Guid> accounts;


    public AccountUpdatePasswordEndpoint(PasswordManager passwordManager, Repository<Account, Guid> accounts)
    {
        this.passwordManager = passwordManager;
        this.accounts = accounts;
    }

    public override void Configure()
    {
        Post("/account/updatePassword");
    }

    public override async Task HandleAsync(AccountUpdatePasswordRequest req, CancellationToken ct)
    {
        var account = await accounts.FindAndEnsureExistenceAsync(req.AccountId, ct);
        account.SetPassword(passwordManager.GetHash(req.Password));

        await accounts.UpdateAsync(account, ct);
        
        await SendOkAsync(ct);
    }
}
