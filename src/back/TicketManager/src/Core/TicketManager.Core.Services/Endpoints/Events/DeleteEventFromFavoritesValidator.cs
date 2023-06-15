using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class DeleteEventFromFavoritesValidator : Validator<DeleteEventFromFavoritesRequest>
{
    public DeleteEventFromFavoritesValidator()
    {
        RuleFor(req => req.EventId)
            .MustAsync(DoesEventExistAsync)
            .WithCode(DeleteEventFromFavoritesRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event does not exist");

        RuleFor(req => req)
            .MustAsync(IsEventFavoriteAsync)
            .WithCode(DeleteEventFromFavoritesRequest.ErrorCodes.EventIsNotAddedToFavorites)
            .WithMessage("Event is already added to favorites");
    }

    private Task<bool> DoesEventExistAsync(Guid id, CancellationToken ct)
    {
        return Resolve<CoreDbContext>()
            .Events
            .AnyAsync(e => e.Id == id && !e.IsDeleted, ct);
    }

    private Task<bool> IsEventFavoriteAsync(DeleteEventFromFavoritesRequest request, CancellationToken ct)
    {
        return Resolve<CoreDbContext>()
            .Users
            .Where(u => u.Id == request.AccountId)
            .SelectMany(u => u.FavoriteEvents)
            .AnyAsync(fe => fe.EventId == request.EventId, ct);
    }
}
