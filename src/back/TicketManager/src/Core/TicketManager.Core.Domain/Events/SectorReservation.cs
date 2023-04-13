using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class SectorReservation: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public Guid EventId { get; private set; }
    public string SectorName { get; private set; } = null!;
    private readonly List<SeatReservation> seatReservations = new();
    public IReadOnlyList<SeatReservation> SeatReservations => seatReservations;
    public DateTime DateModified { get; set; }
    
    public SectorReservation(Guid eventId, string sectorName)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        SectorName = sectorName;
    }

    public SectorReservation() { }

    public void AddSeatReservation(int numberOfSeats)
    {
        seatReservations.Add(new SeatReservation(Id, numberOfSeats));
    }
}
