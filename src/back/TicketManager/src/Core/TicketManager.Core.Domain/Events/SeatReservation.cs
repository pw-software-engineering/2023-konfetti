namespace TicketManager.Core.Domain.Events;

public record SeatReservation(Guid Id, Guid SectorReservationId, int ReservedSeatNumber, DateTime CreationDate)
{ }
