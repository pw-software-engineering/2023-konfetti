using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketDetailsEndpoint : Endpoint<TicketDetailsRequest, TicketDto>
{
    private readonly CoreDbContext dbContext;

    public TicketDetailsEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/ticket");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketDetailsRequest req, CancellationToken ct)
    {
        var dbData = await dbContext
            .Tickets
            .Join(dbContext.Events, t => t.EventId, e => e.Id, (ticket, @event) => new { ticket, @event })
            .Join(dbContext.Sectors, te => te.@event.Id, s => s.EventId, (te, sector) => new { te.ticket, te.@event, sector })
            .ToListAsync(ct);

        if (dbData.Count == 0)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var ticket = dbData
            .Select(tes => tes.ticket)
            .First();

        var @event = dbData
            .Select(tes => tes.@event)
            .First();

        var ticketSector = dbData
            .Select(tes => tes.sector)
            .First(s => s.Id == ticket.SectorId);

        var sectors = dbData
            .Select(tes => tes.sector);

        var response = new TicketDto
        {
            Id = ticket.Id,
            Event = new EventDto
            {
                Id = @event.Id,
                OrganizerId = @event.OrganizerId,
                Name = @event.Name,
                Description = @event.Description,
                Location = @event.Location,
                Date = @event.Date,
                Sectors = sectors
                    .Select(s => new SectorDto
                    {
                        Name = s.Name,
                        PriceInSmallestUnit = s.PriceInSmallestUnit,
                        NumberOfColumns = s.NumberOfColumns,
                        NumberOfRows = s.NumberOfRows,
                    })
                    .ToList(),
            },
            PriceInSmallestUnit = ticket.Seats.Count * ticketSector.PriceInSmallestUnit,
            SectorName = ticketSector.Name,
            Seats = ticket.Seats
                .Select(ts => new TicketSeatDto
                {
                    Row = ts.Row,
                    Column = ts.Column,
                })
                .ToList(),
        };

        await SendOkAsync(response, ct);
    }
}
