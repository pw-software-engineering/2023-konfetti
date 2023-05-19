using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class DeleteEventValidator : Validator<DeleteEventRequest>
{
    public DeleteEventValidator()
    {
        RuleFor(req => req.Id)
            .MustAsync(DoesEventExist)
            .WithCode(DeleteEventRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event does not exits");
    }

    private async Task<bool> DoesEventExist(Guid eventId, CancellationToken ct)
    {
        return await Resolve<CoreDbContext>()
            .Events
            .AnyAsync(e => e.Id == eventId && !e.IsDeleted, ct);
    }
}
