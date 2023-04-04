namespace TicketManager.Core.Domain.Events;

public record Sector(Guid EventId, string Name, int PriceInSmallestUnit, int NumberOfColumns, int NumberOfRows)
{
    public (Guid EventId, string Name) Id => (EventId, Name);
}
