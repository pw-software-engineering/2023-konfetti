namespace TicketManager.Core.Domain.Events;

public record SeatReservation((Guid EventId, string SectorName) SectorId, Guid UserId, int ReservedSeatNumber)
{
    public Guid Id = Guid.NewGuid();
    public DateTime CreationDate = DateTime.UtcNow;
}
