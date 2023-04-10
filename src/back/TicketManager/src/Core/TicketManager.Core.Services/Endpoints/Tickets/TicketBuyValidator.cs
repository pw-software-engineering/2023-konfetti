using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Tickets;
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
            .MustAsync(IsIdPresentAsync)
            .WithCode(TicketBuyRequest.ErrorCodes.EventDoesNotExist)
            .WithMessage("Event with this Id does not exist");
        RuleFor(req => new Tuple<Guid, string>(req.EventId, req.SectorName))
            .MustAsync(IsSectorNameValidAsync)
            .WithCode(TicketBuyRequest.ErrorCodes.SectorNameDoesNotExist)
            .WithMessage("Sector Name does not exist in this event");

    }
    
    private async Task<bool> IsIdPresentAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Events
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    private async Task<bool> IsSectorNameValidAsync(Tuple<Guid, string> req, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Events
            .AnyAsync(e => e.Id == req.Item1 && 
                e.Sectors.Any(s => s.Name == req.Item2), cancellationToken);
    }
}
