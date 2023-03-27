using FastEndpoints;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class CreateEventEndpoint : Endpoint<CreateEventRequest>
{
    private readonly Repository<Event, Guid> events;

    public CreateEventEndpoint(Repository<Event, Guid> events)
    {
        this.events = events;
    }

    public override void Configure()
    {
        Post("/event/create");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(CreateEventRequest req, CancellationToken ct)
    {
        var @event = new Event(
            req.AccountId,
            req.Name,
            req.Description,
            req.Location,
            req.Date,
            req.Sectors
                .Select(s => new Event.SectorData(s.Name, s.PriceInSmallestUnit, s.NumberOfColumns, s.NumberOfRows))
                .ToList());

        await events.AddAsync(@event, ct);

        await SendOkAsync(ct);
    }
}
