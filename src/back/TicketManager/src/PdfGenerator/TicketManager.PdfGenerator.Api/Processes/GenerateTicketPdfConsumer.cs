using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MassTransit;
using TicketManager.Core.Contracts.Processes;
using TicketManager.PdfGenerator.Contracts;

namespace TicketManager.PdfGenerator.Api.Processes;

public class GenerateTicketPdfConsumer : IConsumer<GenerateTicketPdf>
{
    private readonly ILogger<GenerateTicketPdfConsumer> logger;
    private readonly IBus bus;

    public GenerateTicketPdfConsumer(ILogger<GenerateTicketPdfConsumer> logger, IBus bus)
    {
        this.logger = logger;
        this.bus = bus;
    }

    public Task Consume(ConsumeContext<GenerateTicketPdf> context)
    {
        var ticket = context.Message.Ticket;
        var user = context.Message.User;
        var @event = context.Message.Event;
        
        logger.LogInformation("Ticket {TicketId} pdf generation started", ticket.Id);

        PdfWriter writer = new PdfWriter($"ticket-{ticket.Id}.pdf");
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);
        
        Paragraph paragraph = new Paragraph(@event.Name)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(32);
        document.Add(paragraph);
        
        paragraph = new Paragraph($"{@event.Location}, {@event.Date:dd/MM/yyyy HH:mm}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20);
        document.Add(paragraph);
        
        paragraph = new Paragraph(@event.Description)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(14);
        document.Add(paragraph);
        
        paragraph = new Paragraph($"Organizer: {@event.Organizer.DisplayName}\n {@event.Organizer.PhoneNumber}, {@event.Organizer.Address}")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(12);
        document.Add(paragraph);
        document.Add(new Paragraph("\n"));
        document.Add(new Paragraph("\n"));

        paragraph = new Paragraph($"Ticket for {user.FirstName} {user.LastName}, born on {user.BirthDate:dd/MM/yyyy}")
            .SetTextAlignment(TextAlignment.LEFT)
            .SetFontSize(20);
        document.Add(paragraph);

        Table table = new Table(ticket.Seats.Count);
        table.SetWidth(UnitValue.CreatePercentValue(60));
        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
        
        foreach (var seat in ticket.Seats)
        {
            table.AddCell(new Cell()
                .Add(new Paragraph($"Row: {seat.Row} Column: {seat.Column}")
                    .SetTextAlignment(TextAlignment.JUSTIFIED)));
            table.StartNewRow();
        }
        document.Add(table);
        
        document.Close();

        bus.Publish(new SetPdfGenerationFlag
        {
            TicketId = ticket.Id,
        });
        
        return Task.CompletedTask;
    }
}
