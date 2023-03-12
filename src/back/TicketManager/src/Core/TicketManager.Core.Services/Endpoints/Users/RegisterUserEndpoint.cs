using FastEndpoints;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Users;

public class RegisterUserEndpoint : Endpoint<RegisterUserRequest>
{
    private readonly Repository<User, Guid> users;
    private readonly Repository<Account, Guid> accounts;
    private readonly CoreDbContext dbContext;

    public RegisterUserEndpoint(Repository<User, Guid> users, Repository<Account, Guid> accounts, CoreDbContext dbContext)
    {
        this.users = users;
        this.accounts = accounts;
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/user/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
    {
        var user = new User(req.Email, req.FirstName, req.LastName, req.BirthDate);
        var account = user.GetAccount(req.Password);

        users.Add(user);
        accounts.Add(account);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
