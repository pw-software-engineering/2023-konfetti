using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
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
        
        RuleFor(req => req.Name)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(EventUpdateRequest.ErrorCodes.NameIsTooLong);

        RuleFor(req => req.Description)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(EventUpdateRequest.ErrorCodes.DescriptionIsTooLong);

        RuleFor(req => req.Location)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(EventUpdateRequest.ErrorCodes.LocationIsTooLong);

        RuleFor(req => req.Date)
            .Must(IsDateFuture)
            .WithCode(EventUpdateRequest.ErrorCodes.DateIsNotFuture)
            .WithMessage("Date is not future");

        RuleFor(req => req.Sectors)
            .Must(AreSectorsNamesUnique)
            .WithCode(EventUpdateRequest.ErrorCodes.SectorNamesAreNotUnique);

        RuleForEach(req => req.Sectors)
            .SetValidator(new SectorValidator());
    }
    
    private async Task<bool> IsIdPresentAndEventNotOpenAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Events
            .AnyAsync(e => e.Id == id && e.Status != EventStatus.Opened, cancellationToken);
    }
    
    private bool IsDateFuture(DateTime? date)
    {
        return date is null || date > DateTime.UtcNow;
    }
    
    private bool AreSectorsNamesUnique(List<SectorDto>? sectors)
    {
        return sectors is null || sectors.DistinctBy(s => s.Name).Count() == sectors.Count;
    }
}
