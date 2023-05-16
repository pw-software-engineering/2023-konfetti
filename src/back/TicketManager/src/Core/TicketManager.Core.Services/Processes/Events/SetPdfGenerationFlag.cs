using MassTransit;
using TicketManager.Core.Contracts.Processes;
using TicketManager.Core.Domain.Tickets;
using TicketManager.Core.Services.DataAccess.Repositories;

namespace TicketManager.Core.Services.Processes.Events;
public class SetPdfGenerationFlagConsumer : IConsumer<SetPdfGenerationFlag>
{
    private readonly Repository<Ticket, Guid> tickets;

    public SetPdfGenerationFlagConsumer(Repository<Ticket, Guid> tickets)
    {
        this.tickets = tickets;
    }


    public async Task Consume(ConsumeContext<SetPdfGenerationFlag> context)
    {
        var ticket = await tickets.FindAndEnsureExistenceAsync(context.Message.TicketId, context.CancellationToken);
        ticket.SetGeneratedPdf();
        await tickets.UpdateAsync(ticket, context.CancellationToken);
    }
}
