namespace TicketManager.Core.Domain.Events;

public class SeatReservation
{
    public Guid Id { get; set; }
    public Guid SectorReservationId { get; set; }
    public int ReservedSeatNumber { get; set; }
    public DateTime CreationDate { get; set; }
    
    public SeatReservation(Guid sectorReservationId, int reservedSeatNumber)
    {
        Id = Guid.NewGuid();
        SectorReservationId = sectorReservationId;
        ReservedSeatNumber = reservedSeatNumber;
        CreationDate = DateTime.UtcNow;
    }
    
    public SeatReservation() { }
}
