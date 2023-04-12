using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Tickets;

public class SeatReservation: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; set; }
    public (Guid EventId, string SectorName) EventSector { get; set; }
    public int ReservedSeats { get; set; }
    public DateTime DateModified { get; set; }
    
    public SeatReservation(Guid eventId, string sectorName, int numberOfSeats)
    {
        Id = Guid.NewGuid();
        EventSector = (eventId, sectorName);
        ReservedSeats = numberOfSeats;
    }

    public SeatReservation() { }
}
