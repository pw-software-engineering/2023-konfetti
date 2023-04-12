using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class SectorReservation: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; }
    public Guid EventId { get; }
    public string SectorName { get; } = null!;
    public List<SeatReservation> SeatReservations { get; } = new();
    public DateTime DateModified { get; set; }
    
    public SectorReservation(Guid id, Guid eventId, string sectorName)
    {
        Id = id;
        EventId = eventId;
        SectorName = sectorName;
    }

    public SectorReservation() { }

    public void AddSeatReservation(int numberOfSeats)
    {
        SeatReservations.Add(new SeatReservation(numberOfSeats));
    }
}
