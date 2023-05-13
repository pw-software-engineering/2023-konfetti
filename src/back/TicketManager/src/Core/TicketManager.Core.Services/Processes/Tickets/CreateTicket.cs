using MassTransit;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.PdfGenerator.Contracts;

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
    private readonly IBus bus;

    public CreateTicketConsumer(Repository<Ticket, Guid> tickets, IBus bus)
    {
        this.tickets = tickets;
        this.bus = bus;
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

        await bus.Publish(new GenerateTicketPdf());
    }
}
