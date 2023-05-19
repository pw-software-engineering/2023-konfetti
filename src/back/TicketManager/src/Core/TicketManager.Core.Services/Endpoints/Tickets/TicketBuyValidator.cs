using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.Extensions;
using TicketManager.Core.Services.Services.Mockables;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketBuyValidator: Validator<TicketBuyRequest>
{
    private IServiceScopeFactory scopeFactory;
    private MockableCoreDbResolver dbResolver;

    public TicketBuyValidator(IServiceScopeFactory scopeFactory, MockableCoreDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;

        RuleFor(req => req.EventId)
            .MustAsync(IsIdPresentAndEventOpenAsync)
            .WithCode(TicketBuyRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event with this Id does not exist");
        RuleFor(req => req)
            .MustAsync(IsSectorNameValidAsync)
            .WithCode(TicketBuyRequest.ErrorCodes.SectorDoesNotExist)
            .WithMessage("Sector Name does not exist in this event");
        
        RuleFor(req => req.NumberOfSeats)
            .GreaterThan(0)
            .WithCode(TicketBuyRequest.ErrorCodes.NumberOfSeatsIsNotPositive)
            .WithMessage("Number of seats must be positive");
    }
    
    private async Task<bool> IsIdPresentAndEventOpenAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Events
            .AnyAsync(e => e.Id == id && e.Status == EventStatus.Opened && !e.IsDeleted, cancellationToken);
    }

    private async Task<bool> IsSectorNameValidAsync(TicketBuyRequest req, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        return await dbResolver.Resolve(scope)
            .Sectors
            .AnyAsync(s => s.EventId == req.EventId && s.Name == req.SectorName, cancellationToken);
    }
}
