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
            .Select(EventDtoMapper.ToDtoMapper)
            .ToPaginatedResponseAsync(req, ct);

        await SendOkAsync(result, ct);
    }
}
