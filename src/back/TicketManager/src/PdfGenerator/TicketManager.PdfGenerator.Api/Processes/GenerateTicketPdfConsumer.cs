using Azure.Storage.Blobs;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MassTransit;
using TicketManager.Core.Contracts.Processes;
using TicketManager.PdfGenerator.Api.Helpers;
using TicketManager.PdfGenerator.Contracts;

namespace TicketManager.PdfGenerator.Api.Processes;

public class GenerateTicketPdfConsumer : IConsumer<GenerateTicketPdf>
{
    private readonly ILogger<GenerateTicketPdfConsumer> logger;
    private readonly IBus bus;
    private readonly BlobConfiguration blobConfiguration;

    public GenerateTicketPdfConsumer(ILogger<GenerateTicketPdfConsumer> logger, IBus bus, BlobConfiguration blobConfiguration)
    {
        this.logger = logger;
        this.bus = bus;
        this.blobConfiguration = blobConfiguration;
    }

    public Task Consume(ConsumeContext<GenerateTicketPdf> context)
    {
        var ticket = context.Message.Ticket;
        var user = context.Message.User;
        var @event = context.Message.Event;
        
        logger.LogInformation("Ticket {TicketId} pdf generation started", ticket.Id);

        using (var stream = new MemoryStream())
        {
            GeneratePdfFile.Generate(stream, ticket, @event, user);
            stream.Position = 0;
            
            var blobServiceClient = new BlobServiceClient(blobConfiguration.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobConfiguration.ContainerName);
            var blobClient = containerClient.GetBlobClient($"ticket-{ticket.Id}.pdf");

            blobClient.Upload(stream, true);
        }
        
        bus.Publish(new SetPdfGenerationFlag
        {
            TicketId = ticket.Id,
        });
        
        return Task.CompletedTask;
    }
}
