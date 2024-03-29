using System.Globalization;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.HttpClients;

namespace TicketManager.Core.Services.Endpoints.Tickets;

public class TicketBuyEndpoint: Endpoint<TicketBuyRequest, TicketPaymentDto>
{
    private readonly CoreDbContext coreDbContext;
    private readonly PaymentClient paymentClient;
    private readonly Repository<Sector, Guid> sectorRepository;

    public TicketBuyEndpoint(CoreDbContext coreDbContext, PaymentClient paymentClient, Repository<Sector, Guid> sectorRepository)
    {
        this.coreDbContext = coreDbContext;
        this.paymentClient = paymentClient;
        this.sectorRepository = sectorRepository;
    }

    public override void Configure()
    {
        Post("/ticket/buy");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(TicketBuyRequest req, CancellationToken ct)
    {
        var sector = await coreDbContext
            .Sectors
            .AsTracking()
            .Where(s => s.EventId == req.EventId && s.Name == req.SectorName)
            .FirstAsync(ct);

        var freeSeats = sector.GetNumberOfFreeSeats();
        
        if (freeSeats < req.NumberOfSeats)
        {
            // We can't return null paymentId, because specification doesn't allow it.
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        var paymentId = await paymentClient.PostPaymentCreationAsync(ct);

        if (paymentId is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        sector.AddSeatReservation(req.UserId, req.NumberOfSeats, paymentId.Value);
        await sectorRepository.UpdateAsync(sector, ct);

        await SendAsync(new TicketPaymentDto{PaymentId = (Guid)paymentId}, cancellation: ct);
    }
}
