namespace TicketManager.Core.Contracts.Tickets;

public class TicketBuyRequest
{
    public Guid EventId { get; set; }
    public string SectorName { get; set; }
    public int NumberOfSeats { get; set; }
}
