namespace TicketManager.PdfGenerator.Contracts.Ticket;

public class EventDto
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public OrganizerDto Organizer { get; private set; } = null!;
}
