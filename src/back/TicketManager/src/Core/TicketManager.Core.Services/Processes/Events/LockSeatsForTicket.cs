using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Processes.Tickets;

namespace TicketManager.Core.Services.Processes.Events;

public class LockSeatsForTicket
{
    public Guid TicketId { get; set; }
    public Guid PaymentId { get; set; }
}

public class LockSeatsForTicketConsumer : IConsumer<LockSeatsForTicket>
{
    private readonly Repository<Sector, Guid> sectors;
    private readonly CoreDbContext dbContext;
    private readonly IBus bus;

    public LockSeatsForTicketConsumer(Repository<Sector, Guid> sectors, CoreDbContext dbContext, IBus bus)
    {
        this.sectors = sectors;
        this.dbContext = dbContext;
        this.bus = bus;
    }

    public async Task Consume(ConsumeContext<LockSeatsForTicket> context)
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

        var takenSeats = sector.TakeSeats(paymentId);
        await sectors.UpdateAsync(sector, context.CancellationToken);

        await bus.Publish(new CreateTicket
        {
            TicketId = context.Message.TicketId,
            EventId = sector.EventId,
            SectorId = sector.Id,
            UserId = sector.SeatReservations.First(sr => sr.PaymentId == paymentId).UserId,
            Seats = takenSeats.Select(s => new TicketSeat { Row = s.RowNumber, Column = s.ColumnNumber, }).ToList(),
        });
    }
}
