using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Tickets;

public class SeatReservation: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string SectorName { get; set; } = null!;
    public int ReservedSeats { get; set; }
    public DateTime DateModified { get; set; }
    
    public SeatReservation(Guid eventId, string sectorName, int numberOfSeats)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        SectorName = sectorName;
        ReservedSeats = numberOfSeats;
        DateModified = DateTime.UtcNow;
    }

    public SeatReservation() { }
}
