namespace TicketManager.Core.Domain.Events;

public class SeatReservation
{
    public Guid Id { get; }
    public Guid SectorReservationId { get; }
    public int ReservedSeatNumber { get; }
    public DateTime CreationDate { get; }
    
    public SeatReservation(Guid sectorReservationId, int reservedSeatNumber)
    {
        Id = Guid.NewGuid();
        SectorReservationId = sectorReservationId;
        ReservedSeatNumber = reservedSeatNumber;
        CreationDate = DateTime.UtcNow;
    }
    
    public SeatReservation() { }
}
