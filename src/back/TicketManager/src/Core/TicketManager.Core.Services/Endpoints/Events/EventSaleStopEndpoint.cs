using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventSaleStopEndpoint: Endpoint<EventSaleStatusRequest>
{
    private readonly Repository<Event, Guid> events;

    public EventSaleStopEndpoint(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public override void Configure()
    {
        Post("/event/sale/stop");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(EventSaleStatusRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.EventId, ct);
        try
        {
            @event.ChangeEventStatus(EventStatus.Closed);
        }
        catch (Exception)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        await events.UpdateAsync(@event, ct);
        await SendOkAsync(ct);
    }
}
