using FastEndpoints;
using MassTransit;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.Authorizers;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Processes.Events;
using Event = TicketManager.Core.Domain.Events.Event;

namespace TicketManager.Core.Services.Endpoints.Events;

public class DeleteEventEndpoint : Endpoint<DeleteEventRequest>
{
    private readonly Repository<Event, Guid> events;
    private readonly IBus bus;

    public DeleteEventEndpoint(Repository<Event, Guid> events, IBus bus)
    {
        this.events = events;
        this.bus = bus;
    }

    public override void Configure()
    {
        Post("/event/delete");
        Roles(AccountRoles.Admin, AccountRoles.Organizer);
        PreProcessors(new EventAuthorizer<DeleteEventRequest>(events));
    }

    public override async Task HandleAsync(DeleteEventRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);
        
        @event.Delete();
        await events.UpdateAsync(@event, ct);

        if (@event.Date < DateTime.UtcNow)
        {
            await bus.Publish(new RemoveEventTickets() { EventId = @event.Id }, ct);
        }
    }
}
