using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketBuyEndpoint: Endpoint<TicketBuyRequest>
{
    private readonly CoreDbContext coreDbContext;

    public TicketBuyEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public override void Configure()
    {
        Post("/ticket/buy");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketBuyRequest req, CancellationToken ct)
    {
        var @event = await coreDbContext
            .Events
            .Where(e => e.Id == req.EventId)
            .FirstOrDefaultAsync(ct);
        var sector = @event!.Sectors.FirstOrDefault(s => s.Name == req.SectorName);

        await SendOkAsync(ct);
    }
}
