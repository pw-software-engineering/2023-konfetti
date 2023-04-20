using TicketManager.Core.Contracts.Events;

namespace TicketManager.Core.Contracts.Tickets;

public class TicketDto
{
    public Guid Id { get; set; }
    public EventDto Event { get; set; }
    public int PriceInSmallestUnit { get; set; }
    public string SectorName { get; set; }
    public List<TicketSeatDto> Seats { get; set; }
}
