using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Events;

public class MyFavoriteEventsEndpoint : Endpoint<MyFavoriteEventsRequest, List<EventDto>>
{
    private readonly CoreDbContext dbContext;

    public MyFavoriteEventsEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/event/list/my/favorites");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(MyFavoriteEventsRequest req, CancellationToken ct)
    {
        var result = await dbContext
            .Users
            .Where(u => u.Id == req.AccountId)
            .SelectMany(u => u.FavoriteEvents)
            .Join(dbContext.Events, fe => fe.EventId, e => e.Id, (_, e) => new EventDto
            {
                Id = e.Id,
                OrganizerId = e.OrganizerId,
                Name = e.Name,
                Description = e.Description,
                Location = e.Location,
                Date = e.Date,
                Status = (EventStatusDto)e.Status,
                Sectors = dbContext.Sectors
                    .Where(s => s.EventId == e.Id)
                    .Select(s => new SectorDto
                    {
                        Name = s.Name,
                        NumberOfColumns = s.NumberOfColumns,
                        NumberOfRows = s.NumberOfRows,
                        PriceInSmallestUnit = s.PriceInSmallestUnit,
                    })
                    .ToList(),
            })
            .ToListAsync(ct);
        
        await SendOkAsync(ct);
    }
}
