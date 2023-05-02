using FastEndpoints;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Payments;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Processes.Events;
using TicketManager.Core.Services.Services.HttpClients;

namespace TicketManager.Core.Services.Endpoints.Payments;

public class FinishPaymentEndpoint : Endpoint<FinishPaymentRequest, FinishPaymentResponse>
{
    private readonly PaymentClient paymentClient;
    private readonly CoreDbContext dbContext;
    private readonly IBus bus;

    public FinishPaymentEndpoint(PaymentClient paymentClient, CoreDbContext dbContext, IBus bus)
    {
        this.paymentClient = paymentClient;
        this.dbContext = dbContext;
        this.bus = bus;
    }

    public override void Configure()
    {
        Post("/payment/finish");
        Roles(AccountRoles.User);
    }

    public override async Task HandleAsync(FinishPaymentRequest req, CancellationToken ct)
    {
        var status = await paymentClient.GetPaymentStatusAsync(new() { Id = req.PaymentId, }, ct);

        if (status?.Status != PaymentStatusDto.Confirmed)
        {
            await bus.Publish(new UnlockSeatsForInvalidPayment { PaymentId = req.PaymentId }, ct);
            await SendOkAsync(new FinishPaymentResponse(), ct);
            return;
        }

        var doesPaymentReservationExist = await GetPaymentReservationExistenceAsync(req.PaymentId, ct);

        if (!doesPaymentReservationExist)
        {
            await SendOkAsync(new FinishPaymentResponse(), ct);
            return;
        }

        var ticketId = Guid.NewGuid();
        await bus.Publish(new LockSeatsForTicket { PaymentId = req.PaymentId, TicketId = ticketId }, ct);
        await SendOkAsync(new FinishPaymentResponse { TicketId = ticketId }, ct);
    }

    private async Task<bool> GetPaymentReservationExistenceAsync(Guid paymentId, CancellationToken ct)
    {
        var sector = await dbContext
            .Sectors
            .FirstOrDefaultAsync(s => s.SeatReservations.Any(sr => sr.PaymentId == paymentId), ct);

        if (sector is null)
        {
            return false;
        }

        return sector.SeatReservations.Any(sr => sr.PaymentId == paymentId && sr.IsCurrent);
    }
}
