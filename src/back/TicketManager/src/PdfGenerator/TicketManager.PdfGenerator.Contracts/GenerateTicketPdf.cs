using TicketManager.PdfGenerator.Contracts.Ticket;

namespace TicketManager.PdfGenerator.Contracts;

public class GenerateTicketPdf
{
    public TicketDto Ticket { get; set; } = null!;
    public UserDto User { get; set; } = null!;
    public EventDto Event { get; set; } = null!;
}
