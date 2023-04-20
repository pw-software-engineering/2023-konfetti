namespace TicketManager.Core.Domain.Events;

public record SeatReservation(Guid SectorId, Guid UserId, int ReservedSeatNumber, Guid PaymentId)
{
    public readonly static TimeSpan ReservationLifetime = TimeSpan.FromMinutes(35);
    
    public Guid Id { get; private init; } = Guid.NewGuid();
    public DateTime CreationDate { get; private init; } = DateTime.UtcNow;
    public bool IsClosed { get; private set; }
    
    public bool IsExpired => CreationDate + ReservationLifetime <= DateTime.UtcNow;

    public void Close()
    {
        IsClosed = true;
    }
}
