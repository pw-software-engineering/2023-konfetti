using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Services.HttpClients;

namespace TicketManager.Core.Services.Endpoints.Tickets;

// TODO: Currently this endpoint doesn't follow specification - it returns paymentId
public class TicketBuyEndpoint: Endpoint<TicketBuyRequest, TicketPaymentDto>
{
    private readonly CoreDbContext coreDbContext;
    private readonly PaymentClient paymentClient;
    
    public TicketBuyEndpoint(CoreDbContext coreDbContext, PaymentClient paymentClient)
    {
        this.coreDbContext = coreDbContext;
        this.paymentClient = paymentClient;
    }

    public override void Configure()
    {
        Post("/ticket/buy");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketBuyRequest req, CancellationToken ct)
    {
        // This code really doesn't matter for now, it can wait for further tasks.
        var @event = await coreDbContext
            .Events
            .Where(e => e.Id == req.EventId)
            .FirstOrDefaultAsync(ct);
        var sector = @event!.Sectors.FirstOrDefault(s => s.Name == req.SectorName);
        
        // TODO: lock seats
        
        
        var paymentId = await paymentClient.PostPaymentCreation(ct);

        if (paymentId is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendAsync(new TicketPaymentDto{PaymentId = (Guid)paymentId}, cancellation: ct);
    }
}
