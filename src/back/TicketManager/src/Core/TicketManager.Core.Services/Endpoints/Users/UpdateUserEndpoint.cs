using FastEndpoints;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Users;

public class UpdateUserEndpoint : Endpoint<UpdateUserRequest>
{
    private readonly Repository<User, Guid> users;
    private readonly Repository<Account, Guid> accounts;

    public UpdateUserEndpoint(Repository<User, Guid> users, Repository<Account, Guid> accounts)
    {
        this.users = users;
        this.accounts = accounts;
    }

    public override void Configure()
    {
        Post("/user/update");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        var user = await users.FindAndEnsureExistenceAsync(req.AccountId, ct);
        
        user.Update(
            req.Email ?? user.Email,
            req.FirstName ?? user.FirstName,
            req.LastName ?? user.LastName,
            req.BirthDate ?? user.BirthDate);

        if (req.Email is not null)
        {
            var account = await accounts.FindAndEnsureExistenceAsync(req.AccountId, ct);
            accounts.Update(account);
        }

        await users.UpdateAsync(user, ct);

        await SendOkAsync(ct);
    }
}
