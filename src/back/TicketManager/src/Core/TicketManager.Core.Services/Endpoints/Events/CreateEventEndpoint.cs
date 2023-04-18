using FastEndpoints;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class CreateEventEndpoint : Endpoint<CreateEventRequest, IdResponse>
{
    private readonly Repository<Event, Guid> events;
    private readonly Repository<Sector, Guid> sectors;

    public CreateEventEndpoint(Repository<Event, Guid> events, Repository<Sector, Guid> sectors)
    {
        this.events = events;
        this.sectors = sectors;
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
            req.Date);

        foreach (var sector in req.Sectors)
        {
            sectors.Add(new Sector(
                @event.Id, 
                sector.Name, 
                sector.PriceInSmallestUnit,
                sector.NumberOfColumns,
                sector.NumberOfRows));
        }

        await events.AddAsync(@event, ct);

        await SendAsync(new IdResponse { Id = @event.Id }, cancellation: ct);
    }
}
