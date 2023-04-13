using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Services.HttpClients;

namespace TicketManager.Core.Services.Endpoints.Tickets;

// TODO: Currently this endpoint doesn't follow specification - it returns paymentId
public class TicketBuyEndpoint: Endpoint<TicketBuyRequest, TicketPaymentDto>
{
    private readonly CoreDbContext coreDbContext;
    private readonly PaymentClient paymentClient;
    private readonly Repository<SectorReservation, Guid> sectorReservationRepository;

    public TicketBuyEndpoint(CoreDbContext coreDbContext, PaymentClient paymentClient, Repository<SectorReservation, Guid> sectorReservationRepository)
    {
        this.coreDbContext = coreDbContext;
        this.paymentClient = paymentClient;
        this.sectorReservationRepository = sectorReservationRepository;
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

        // var sectorReservation = await coreDbContext.SectorReservations
        //     .Where(sr => sr.EventId == req.EventId && sr.SectorName == req.SectorName)
        //     .FirstOrDefaultAsync(ct);
        //
        var sectorReservationId = await coreDbContext.SectorReservations
            .Where(sr => sr.EventId == req.EventId && sr.SectorName == req.SectorName).Select(sr => sr.Id)
            .FirstOrDefaultAsync(ct);
        
        bool updateRepository = true;
        var sectorReservation = await sectorReservationRepository.FindAsync(sectorReservationId, ct);

        if (sectorReservation is null)
        {
            sectorReservation = new SectorReservation(req.EventId, req.SectorName);
            updateRepository = false;
        }
        
        var freeSeats = sector!.NumberOfSeats - sectorReservation.SeatReservations.Sum(sr => sr.ReservedSeatNumber);
        
        if (freeSeats < req.NumberOfSeats)
        {
            // We can't return null paymentId, because specification doesn't allow it.
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        sectorReservation.AddSeatReservation(req.NumberOfSeats);
        if (updateRepository)
        {
            await sectorReservationRepository.UpdateAsync(sectorReservation, ct);
        }
        else
        {
            await sectorReservationRepository.AddAsync(sectorReservation, ct);
        }
        
        var paymentId = await paymentClient.PostPaymentCreationAsync(ct);

        if (paymentId is null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendAsync(new TicketPaymentDto{PaymentId = (Guid)paymentId}, cancellation: ct);
    }
}
