using MassTransit;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Processes.Tickets;

namespace TicketManager.Core.Services.Processes.Events;

public class RemoveEventTickets
{
    public Guid EventId { get; set; }
}

public class RemoveEventTicketsConsumer : IConsumer<RemoveEventTickets>
{
    private readonly CoreDbContext dbContext;
    private readonly IBus bus;
    
    public RemoveEventTicketsConsumer(CoreDbContext dbContext, IBus bus)
    {
        this.dbContext = dbContext;
        this.bus = bus;
    }

    public async Task Consume(ConsumeContext<RemoveEventTickets> context)
    {
        var ticketsDeleteList = await dbContext
            .Tickets
            .Where(t => t.EventId == context.Message.EventId)
            .Select(t => new DeleteTicket
            {
                TicketId = t.Id
            })
            .ToListAsync(context.CancellationToken);
        var sectorsDeleteList = await dbContext
            .Sectors
            .Where(s => s.EventId == context.Message.EventId)
            .Select(s => new DeleteSectorReservation
            {
                SectorId = s.Id
            })
            .ToListAsync(context.CancellationToken);
        
        
        await bus.PublishBatch(ticketsDeleteList);
        await bus.PublishBatch(sectorsDeleteList);
    }
}
