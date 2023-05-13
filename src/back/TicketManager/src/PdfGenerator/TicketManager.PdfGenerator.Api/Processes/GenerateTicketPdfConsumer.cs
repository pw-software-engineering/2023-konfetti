using MassTransit;
using TicketManager.PdfGenerator.Contracts;

namespace TicketManager.PdfGenerator.Api.Processes;

public class GenerateTicketPdfConsumer : IConsumer<GenerateTicketPdf>
{
    public Task Consume(ConsumeContext<GenerateTicketPdf> context)
    {
        return Task.CompletedTask;
    }
}
