namespace TicketManager.PdfGenerator.Contracts.Ticket;

public class EventDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime Date { get; set; }
    public OrganizerDto Organizer { get; set; } = null!;
}
