using FastEndpoints;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.DtoMappers;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class ListEventsEndpoint : Endpoint<ListEventsRequest, PaginatedResponse<EventDto>>
{
    private readonly CoreDbContext dbContext;

    public ListEventsEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/event/list");
        Roles(AccountRoles.User, AccountRoles.Organizer, AccountRoles.Admin);
    }

    public override async Task HandleAsync(ListEventsRequest req, CancellationToken ct)
    {
        var result = await dbContext
            .Events
            .Select(EventDtoMapper.ToDtoMapper)
            .ToPaginatedResponseAsync(req, ct);

        await SendOkAsync(result, ct);
    }
}
