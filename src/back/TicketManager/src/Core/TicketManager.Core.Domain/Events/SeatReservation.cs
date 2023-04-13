namespace TicketManager.Core.Domain.Events;

public record SeatReservation
{
    public Guid Id { get; private init; }
    public Guid SectorReservationId { get; private set; }
    public int ReservedSeatNumber { get; private set; }
    public DateTime CreationDate { get; private set; }
    
    public SeatReservation(Guid sectorReservationId, int reservedSeatNumber)
    {
        Id = Guid.NewGuid();
        SectorReservationId = sectorReservationId;
        ReservedSeatNumber = reservedSeatNumber;
        CreationDate = DateTime.UtcNow;
    }
}
