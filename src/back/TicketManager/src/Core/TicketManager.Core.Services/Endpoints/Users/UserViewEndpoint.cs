using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Users;

public class UserViewEndpoint: Endpoint<UserViewRequest, UserDto>
{
    private readonly CoreDbContext coreDbContext;


    public UserViewEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public override void Configure()
    {
        Get("/user/view");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(UserViewRequest req, CancellationToken ct)
    {
        var response = await coreDbContext
            .Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                BirthDate = u.BirthDate
            })
            .FirstOrDefaultAsync(u => u.Id == req.AccountId, ct);
        if (response is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendAsync(response, cancellation: ct);
    }
}
