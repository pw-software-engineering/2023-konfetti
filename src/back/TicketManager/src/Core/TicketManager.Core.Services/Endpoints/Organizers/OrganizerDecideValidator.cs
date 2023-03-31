using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.Services.Mockables;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class OrganizerDecideValidator : Validator<OrganizerDecideRequest>
{
    private IServiceScopeFactory scopeFactory;
    private MockableCoreDbResolver dbResolver;
    
    public OrganizerDecideValidator(IServiceScopeFactory scopeFactory, MockableCoreDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;

        RuleFor(req => req.OrganizerId)
            .MustAsync(IsIdPresentAsync)
            .WithCode(OrganizerDecideRequest.ErrorCodes.OrganizerDoesNotExist)
            .WithMessage("Organizer with this Id does not exist")
            .MustAsync(IsUnverifiedAsync)
            .WithCode(OrganizerDecideRequest.ErrorCodes.OrganizerAlreadyVerified)
            .WithMessage("Organizer has been already verified");
    }
    
    private async Task<bool> IsIdPresentAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Organizers
            .AnyAsync(o => o.Id == id, cancellationToken);
    }
    
    private async Task<bool> IsUnverifiedAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Organizers
            .AnyAsync(o => o.Id == id && o.VerificationStatus == VerificationStatus.Unverified, cancellationToken);
    }
}
