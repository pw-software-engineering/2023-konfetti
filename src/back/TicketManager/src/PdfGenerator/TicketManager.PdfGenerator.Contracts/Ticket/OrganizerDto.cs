namespace TicketManager.PdfGenerator.Contracts.Ticket;

public class OrganizerDto
{
    public string DisplayName { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
}
