using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TicketManager.PdfGenerator.Contracts.Ticket;

namespace TicketManager.PdfGenerator.Api.Helpers;

public static class GeneratePdfFile
{
    public static void Generate(MemoryStream stream, TicketDto ticket, EventDto @event, UserDto user)
    {
        PdfWriter writer = new PdfWriter(stream);
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);
        
        pdf.SetCloseReader(false);
        pdf.SetCloseWriter(false);
        
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
    }
}
