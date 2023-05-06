using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Events;

public class UnlockSeatsForInvalidPayment
{
    public Guid PaymentId { get; set; }
}

public class UnlockSeatsForInvalidPaymentConsumer : IConsumer<UnlockSeatsForInvalidPayment>
{
    private readonly Repository<Sector, Guid> sectors;
    private readonly CoreDbContext dbContext;

    public UnlockSeatsForInvalidPaymentConsumer(Repository<Sector, Guid> sectors, CoreDbContext dbContext)
    {
        this.sectors = sectors;
        this.dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UnlockSeatsForInvalidPayment> context)
    {
        var paymentId = context.Message.PaymentId;
        var sector = await dbContext
            .Sectors
            .AsTracking()
            .Where(s => s.SeatReservations.Any(sr => sr.PaymentId == paymentId))
            .FirstOrDefaultAsync(context.CancellationToken);

        if (sector is null)
        {
            return;
        }
        
        sector.CloseReservation(paymentId);
        await sectors.UpdateAsync(sector, context.CancellationToken);
    }
}
