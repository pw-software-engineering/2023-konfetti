using FastEndpoints;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.DtoMappers;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class ListEventsForOrganizerEndpoint: Endpoint<ListEventForOrganizerRequest, PaginatedResponse<EventDto>>
{
    private readonly CoreDbContext dbContext;

    public ListEventsForOrganizerEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/event/organizer/my/list");
        Roles(AccountRoles.Organizer);
    }

    public override async Task HandleAsync(ListEventForOrganizerRequest req, CancellationToken ct)
    {
        var result = await dbContext
            .Events
            .Where(e => e.OrganizerId == req.OrganizerId)
            .GroupJoin(dbContext.Sectors, e => e.Id, s => s.EventId, (e, s) => new { Event = e, Sectors = s })
            .Select(e => new EventDto
            {
                Id = e.Event.Id,
                Name = e.Event.Name,
                Description = e.Event.Description,
                Location = e.Event.Location,
                Date = e.Event.Date,
                Sectors = e.Sectors.Select(s => new SectorDto
                {
                    Name = s.Name,
                    PriceInSmallestUnit = s.PriceInSmallestUnit,
                    NumberOfColumns = s.NumberOfColumns,
                    NumberOfRows = s.NumberOfRows,
                }).ToList()
            })
            .ToPaginatedResponseAsync(req, ct);

        await SendOkAsync(result, ct);
    }
}
