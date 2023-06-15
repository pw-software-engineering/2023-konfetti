using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class AddEventToFavoritesEndpoint : Endpoint<AddEventToFavoritesRequest>
{
    private readonly Repository<User, Guid> users;

    public AddEventToFavoritesEndpoint(Repository<User, Guid> users)
    {
        this.users = users;
    }

    public override void Configure()
    {
        Post("/event/favorite/add");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(AddEventToFavoritesRequest req, CancellationToken ct)
    {
        var user = await users.FindAndEnsureExistenceAsync(req.AccountId, ct);
        
        user.AddFavoriteEvent(new(req.EventId));
        await users.UpdateAsync(user, ct);
        
        await SendOkAsync(ct);
    }
}
