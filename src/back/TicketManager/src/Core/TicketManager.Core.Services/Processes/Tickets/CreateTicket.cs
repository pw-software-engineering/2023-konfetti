using MassTransit;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Tickets;

public class CreateTicket
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public Guid SectorId { get; set; }
    public List<TicketSeat> Seats { get; set; } = null!;
}

public class TicketSeat
{
    public int Row { get; set; }
    public int Column { get; set; }
}

public class CreateTicketConsumer : IConsumer<CreateTicket>
{
    private readonly Repository<Ticket, Guid> tickets;

    public CreateTicketConsumer(Repository<Ticket, Guid> tickets)
    {
        this.tickets = tickets;
    }

    public async Task Consume(ConsumeContext<CreateTicket> context)
    {
        var message = context.Message;
        var ticket = new Ticket(
            message.TicketId,
            message.UserId,
            message.EventId,
            message.SectorId,
            message.Seats.Select(s => new Domain.Tickets.TicketSeat(s.Row, s.Column)));

        await tickets.AddAsync(ticket, context.CancellationToken);
    }
}
