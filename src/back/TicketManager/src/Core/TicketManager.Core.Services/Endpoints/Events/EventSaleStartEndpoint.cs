using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventSaleStartEndpoint: Endpoint<EventSaleStatusRequest>
{
    private readonly Repository<Event, Guid> events;

    public EventSaleStartEndpoint(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public override void Configure()
    {
        Post("/event/sale/start");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(EventSaleStatusRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.EventId, ct);
        try
        {
            @event.ChangeEventStatus(EventStatus.Opened);
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
