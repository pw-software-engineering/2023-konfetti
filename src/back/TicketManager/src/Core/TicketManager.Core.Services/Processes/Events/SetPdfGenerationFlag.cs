using MassTransit;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Events;

public class SetPdfGenerationFlag
{
    public Guid EventId { get; set; }
}

public class SetPdfGenerationFlagConsumer : IConsumer<SetPdfGenerationFlag>
{
    private readonly Repository<Ticket, Guid> tickets;

    public SetPdfGenerationFlagConsumer(Repository<Ticket, Guid> tickets)
    {
        this.tickets = tickets;
    }


    public async Task Consume(ConsumeContext<SetPdfGenerationFlag> context)
    {
        var ticket = await tickets.FindAndEnsureExistenceAsync(context.Message.EventId, context.CancellationToken);
        ticket.SetGeneratedPdf();
        await tickets.UpdateAsync(ticket, context.CancellationToken);
    }
}
