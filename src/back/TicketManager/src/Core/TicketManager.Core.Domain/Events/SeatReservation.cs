namespace TicketManager.Core.Domain.Events;

public class SeatReservation
{
    public Guid Id { get; }
    public int ReservedSeatNumber { get; }
    public DateTime CreationDate { get; }
    
    public SeatReservation(int reservedSeatNumber)
    {
        Id = Guid.NewGuid();
        ReservedSeatNumber = reservedSeatNumber;
        CreationDate = DateTime.UtcNow;
    }
    
    public SeatReservation() { }
}
