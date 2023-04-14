using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class SectorReservation: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    private readonly List<SeatReservation> seatReservations = new();
    public Guid Id { get; private init; }
    public Guid EventId { get; private set; }
    public string SectorName { get; private set; } = null!;
    public IReadOnlyList<SeatReservation> SeatReservations => seatReservations;
    public DateTime DateModified { get; set; }
    
    public SectorReservation(Guid eventId, string sectorName)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        SectorName = sectorName;
    }

    public SectorReservation() { }

    public SeatReservation AddSeatReservation(int numberOfSeats)
    {
        var seatReservation = new SeatReservation(Guid.NewGuid(), Id, numberOfSeats, DateTime.UtcNow);
        seatReservations.Add(seatReservation);
        return seatReservation;
    }
}
