using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventDecideValidator : Validator<EventDecideRequest>
{
    public EventDecideValidator()
    {
        RuleFor(req => req.Id)
            .MustAsync(DoesEventExist)
            .WithCode(EventDecideRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event does not exist")
            .MustAsync(IsEventUnverified)
            .WithCode(EventDecideRequest.ErrorCodes.EventIsAlreadyVerified)
            .WithMessage("Event is already verified");
    }
    
    private async Task<bool> DoesEventExist(Guid eventId, CancellationToken ct)
    {
        return await Resolve<CoreDbContext>()
            .Events
            .AnyAsync(e => e.Id == eventId && !e.IsDeleted, ct);
    }
    
    private async Task<bool> IsEventUnverified(Guid eventId, CancellationToken ct)
    {
        return await Resolve<CoreDbContext>()
            .Events
            .AllAsync(e => e.Id != eventId || e.Status == EventStatus.Unverified, ct);
    }
}
