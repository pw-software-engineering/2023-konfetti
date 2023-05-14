namespace TicketManager.PdfGenerator.Contracts.Ticket;

public class UserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}
