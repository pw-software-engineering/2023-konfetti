using MassTransit;
using TicketManager.PdfGenerator.Contracts;

namespace TicketManager.PdfGenerator.Api.Processes;

public class GenerateTicketPdfConsumer : IConsumer<GenerateTicketPdf>
{
    private readonly ILogger<GenerateTicketPdfConsumer> logger;

    public GenerateTicketPdfConsumer(ILogger<GenerateTicketPdfConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<GenerateTicketPdf> context)
    {
        logger.LogInformation("Ticket {TicketId} pdf generation started", Guid.NewGuid());
        return Task.CompletedTask;
    }
}
