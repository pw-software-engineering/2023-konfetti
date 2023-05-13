using System.Net;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Events;

public class GetEventEndpoint: Endpoint<GetEventRequest, EventDto>
{
    private readonly CoreDbContext dbContext;

    public GetEventEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/event");
        Roles(AccountRoles.Admin, AccountRoles.Organizer, AccountRoles.User);
    }

    public override async Task HandleAsync(GetEventRequest req, CancellationToken ct)
    {
        var result = await dbContext
            .Events
            .Where(e => e.Id == req.EventId)
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
            .FirstOrDefaultAsync(ct);

        if (result is not null)
        {
            await SendOkAsync(result, ct);
        }
    }
}
