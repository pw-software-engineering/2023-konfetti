using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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

        PdfWriter writer = new PdfWriter("./demo.pdf");
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);
        Paragraph header = new Paragraph("HEADER")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);
        Paragraph body = new Paragraph("Some super ticket")
            .SetTextAlignment(TextAlignment.LEFT)
            .SetFontSize(14);

        document.Add(header);
        document.Add(body);
        document.Close();
        
        return Task.CompletedTask;
    }
}
