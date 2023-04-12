using System.Net;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.HttpClients;

namespace TicketManager.Core.Services.Endpoints.Tickets;

// TODO: Currently this endpoint doesn't follow specification - it returns paymentId
public class TicketBuyEndpoint: Endpoint<TicketBuyRequest, TicketPaymentDto>
{
    private readonly CoreDbContext coreDbContext;
    private readonly PaymentClient paymentClient;
    private readonly Repository<SeatReservation, Guid> seatReservationRepository;

    public TicketBuyEndpoint(CoreDbContext coreDbContext, PaymentClient paymentClient, Repository<SeatReservation, Guid> seatReservationRepository)
    {
        this.coreDbContext = coreDbContext;
        this.paymentClient = paymentClient;
        this.seatReservationRepository = seatReservationRepository;
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
        
        var freeSeats = sector!.NumberOfSeats - await coreDbContext
            .SeatReservations
            .Where(sr => sr.EventId == req.EventId && sr.SectorName == req.SectorName)
            .SumAsync(sr => sr.ReservedSeats, ct);

        if (freeSeats < req.NumberOfSeats)
        {
            // We can't return null paymentId, because specification doesn't allow it.
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        // assume that seats are not being reserved by somebody else
        await seatReservationRepository.AddAsync(new SeatReservation(req.EventId, req.SectorName, req.NumberOfSeats), ct);
        
        var paymentId = await paymentClient.PostPaymentCreationAsync(ct);

        if (paymentId is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendAsync(new TicketPaymentDto{PaymentId = (Guid)paymentId}, cancellation: ct);
    }
}
