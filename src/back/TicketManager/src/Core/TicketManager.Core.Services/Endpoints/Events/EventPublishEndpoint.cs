using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventPublishEndpoint: Endpoint<EventStatusManipulationRequest>
{
    private readonly Repository<Event, Guid> events;

    public EventPublishEndpoint(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public override void Configure()
    {
        Post("/event/publish");
        Roles(AccountRoles.Organizer, AccountRoles.Admin);
    }

    public override async Task HandleAsync(EventStatusManipulationRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);
        @event.ChangeEventStatus(EventStatus.Published);
        await events.UpdateAsync(@event, ct);
        await SendOkAsync(ct);
    }
}
