using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class DeleteEventFromFavoritesEndpoint : Endpoint<DeleteEventFromFavoritesRequest>
{
    private readonly Repository<User, Guid> users;

    public DeleteEventFromFavoritesEndpoint(Repository<User, Guid> users)
    {
        this.users = users;
    }

    public override void Configure()
    {
        Post("/event/favorite/delete");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(DeleteEventFromFavoritesRequest req, CancellationToken ct)
    {
        var user = await users.FindAndEnsureExistenceAsync(req.AccountId, ct);
        
        user.RemoveEventFromFavorites(req.EventId);
        await users.UpdateAsync(user, ct);
        
        await SendOkAsync(ct);
    }
}
