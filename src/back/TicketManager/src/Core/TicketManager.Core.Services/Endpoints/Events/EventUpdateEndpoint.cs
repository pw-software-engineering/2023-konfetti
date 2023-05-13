using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Endpoints.Events;

public class EventUpdateEndpoint: Endpoint<EventUpdateRequest>
{
    private readonly Repository<Event, Guid> events;
    private readonly Repository<Sector, Guid> sectors;
    private readonly CoreDbContext dbContext;

    public EventUpdateEndpoint(Repository<Event, Guid> events, Repository<Sector, Guid> sectors, CoreDbContext dbContext)
    {
        this.events = events;
        this.sectors = sectors;
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/event/update");
        Roles(AccountRoles.Admin, AccountRoles.Organizer);
    }

    public override async Task HandleAsync(EventUpdateRequest req, CancellationToken ct)
    {
        var @event = await events.FindAndEnsureExistenceAsync(req.Id, ct);
        var updateEvent = false;
        
        if (req.Name is not null)
        {
            @event.UpdateName(req.Name);
            updateEvent = true;
        }
        if (req.Description is not null)
        {
            @event.UpdateName(req.Description);
            updateEvent = true;
        }
        if (req.Location is not null)
        {
            @event.UpdateName(req.Location);
            updateEvent = true;
        }
        if (req.Date is not null)
        {
            @event.UpdateDate((DateTime)req.Date);
            updateEvent = true;
        }

        if (req.Sectors is not null)
        {
            var sectorsList = await dbContext.Sectors.AsTracking().Where(s => s.EventId == req.Id).ToListAsync(ct);

            foreach (var sector in sectorsList)
            {
                var sectorDto = req.Sectors.Find(s => s.Name == sector.Name);
                if (sectorDto is not null)
                {
                    req.Sectors.Remove(sectorDto);
                    if (sector.Update(sectorDto.PriceInSmallestUnit, sectorDto.NumberOfColumns,
                            sectorDto.NumberOfRows))
                    {
                        sectors.Update(sector);
                    }
                }
                else
                {
                    sectors.Delete(sector);
                }
            }

            foreach (var sectorDto in req.Sectors)
            {
                var sector = new Sector(@event.Id, sectorDto.Name, sectorDto.PriceInSmallestUnit,
                    sectorDto.NumberOfColumns, sectorDto.NumberOfRows);
                sectors.Add(sector);
            }
        }

        if (updateEvent)
        {
            events.Update(@event);
        }
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}
