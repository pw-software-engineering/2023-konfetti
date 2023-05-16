using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventDecideEndpoint : Endpoint<EventDecideRequest>
{
    private readonly Repository<Event, Guid> events;

    public EventDecideEndpoint(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public override void Configure()
    {
        Post("/event/decide");
        Roles(AccountRoles.Admin);
    }

    public override async Task HandleAsync(EventDecideRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);

        if (req.IsAccepted)
        {
            @event.ChangeEventStatus(EventStatus.Verified);
        }
        else
        {
            // delete it when possible
        }

        await events.UpdateAsync(@event, ct);
        
        await SendOkAsync(ct);
    }
}
