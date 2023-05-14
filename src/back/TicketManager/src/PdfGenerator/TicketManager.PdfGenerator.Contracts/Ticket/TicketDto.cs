namespace TicketManager.PdfGenerator.Contracts.Ticket;

public class TicketDto
{
    public Guid Id { get; set; }
    public string SectorName { get; set; }
    public List<TicketSeatDto> Seats { get; set; } = null!;
}
