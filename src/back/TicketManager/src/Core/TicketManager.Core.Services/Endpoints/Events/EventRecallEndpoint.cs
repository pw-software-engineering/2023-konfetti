using FastEndpoints;
using MassTransit;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Processes.Events;
using Event = TicketManager.Core.Domain.Events.Event;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventRecallEndpoint: Endpoint<EventStatusManipulationRequest>
{
    private readonly Repository<Event, Guid> events;
    private readonly IBus bus;
    public EventRecallEndpoint(Repository<Event, Guid> events, IBus bus)
    {
        this.events = events;
        this.bus = bus;
    }

    public override void Configure()
    {
        Post("/event/recall");
        Roles(AccountRoles.Organizer, AccountRoles.Admin);
    }

    public override async Task HandleAsync(EventStatusManipulationRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);
        try
        {
            @event.ChangeEventStatus(EventStatus.Recalled);
        }
        catch (Exception)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        await events.UpdateAsync(@event, ct);
        await bus.Publish(new RemoveEventTickets{EventId = req.Id}, ct);
        await SendOkAsync(ct);
    }
}
