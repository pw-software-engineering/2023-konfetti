namespace TicketManager.Core.Domain.Events;

public record SeatReservation(Guid SectorId, Guid UserId, int ReservedSeatNumber)
{
    public Guid Id { get; private init; }= Guid.NewGuid();
    public DateTime CreationDate { get; private init; }= DateTime.UtcNow;
}
