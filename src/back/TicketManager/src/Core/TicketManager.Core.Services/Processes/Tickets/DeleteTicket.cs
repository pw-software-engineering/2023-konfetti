using MassTransit;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Tickets;

public class DeleteTicket
{
    public Guid TicketId { get; set; }
}

public class DeleteTicketConsumer : IConsumer<DeleteTicket>
{
    private readonly Repository<Ticket, Guid> tickets;

    public DeleteTicketConsumer(Repository<Ticket, Guid> tickets)
    {
        this.tickets = tickets;
    }

    public async Task Consume(ConsumeContext<DeleteTicket> context)
    {
        var ticket = await tickets.FindAndEnsureExistenceAsync(context.Message.TicketId, context.CancellationToken);
        await tickets.DeleteAsync(ticket, context.CancellationToken);
    }
}
