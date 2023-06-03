using FastEndpoints;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketListEndpoint : Endpoint<TicketListRequest, PaginatedResponse<TicketDto>>
{
    private readonly CoreDbContext dbContext;

    public TicketListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/ticket/list");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketListRequest req, CancellationToken ct)
    {
        var result = await dbContext
            .Tickets
            .Where(t => t.UserId == req.AccountId)
            .Join(dbContext.Sectors, t => t.SectorId, s => s.Id, (ticket, sector) => new { ticket, sector, })
            .Select(ts => new TicketDto
            {
                Id = ts.ticket.Id,
                PriceInSmallestUnit = ts.ticket.Seats.Count * ts.sector.PriceInSmallestUnit,
                SectorName = ts.sector.Name,
                Seats = ts.ticket.Seats
                    .Select(s => new TicketSeatDto
                    {
                        Column = s.Column,
                        Row = s.Row,
                    })
                    .ToList(),
                Event = dbContext
                    .Events
                    .Where(e => ts.ticket.EventId == e.Id)
                    .GroupJoin(dbContext.Sectors, e => e.Id, s => s.EventId, (e, s) => new { Event = e, Sectors = s })
                    .Select(e => new EventDto
                    {
                        Id = e.Event.Id,
                        OrganizerId = e.Event.OrganizerId,
                        Name = e.Event.Name,
                        Description = e.Event.Description,
                        Location = e.Event.Location,
                        Date = e.Event.Date,
                        Status = (EventStatusDto)e.Event.Status,
                        Sectors = e.Sectors.Select(s => new SectorDto
                        {
                            Name = s.Name,
                            PriceInSmallestUnit = s.PriceInSmallestUnit,
                            NumberOfColumns = s.NumberOfColumns,
                            NumberOfRows = s.NumberOfRows,
                        }).ToList()
                    })
                    .First(),
            })
            .ToPaginatedResponseAsync(req, ct);

        await SendOkAsync(result, ct);
    }
}
