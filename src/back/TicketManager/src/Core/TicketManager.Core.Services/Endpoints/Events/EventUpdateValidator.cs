using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.Extensions;
using TicketManager.Core.Services.Services.Mockables;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventUpdateValidator: Validator<EventUpdateRequest>
{
    private IServiceScopeFactory scopeFactory;
    private MockableCoreDbResolver dbResolver;
    
    public EventUpdateValidator(IServiceScopeFactory scopeFactory, MockableCoreDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;
        
        RuleFor(req => req.Id)
            .MustAsync(IsIdPresentAndEventNotOpenAsync)
            .WithCode(TicketBuyRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event with this Id does not exist");
    }
    
    private async Task<bool> IsIdPresentAndEventNotOpenAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Events
            .AnyAsync(e => e.Id == id && e.Status != EventStatus.Opened, cancellationToken);
    }
}
