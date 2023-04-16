using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Sector: 
    IAggregateRoot<(Guid EventId, string Name)>, IOptimisticConcurrent
{
    public (Guid EventId, string Name) Id => (EventId, Name);
    public Guid EventId { get; private init; }
    public string Name { get; private init; } = null!;
    public int PriceInSmallestUnit { get; private set; }
    public int NumberOfColumns { get; private set; }
    public int NumberOfRows { get; private set; }
    public int NumberOfSeats => NumberOfRows * NumberOfColumns;
    private List<SeatReservation> seatReservations = new();
    public IReadOnlyCollection<SeatReservation> SeatReservations => seatReservations.AsReadOnly();
    public DateTime DateModified { get; set; }
    
    public Sector(Guid eventId, string name, int priceInSmallestUnit, int numberOfColumns, int numberOfRows)
    {
        EventId = eventId;
        Name = name;
        PriceInSmallestUnit = priceInSmallestUnit;
        NumberOfColumns = numberOfColumns;
        NumberOfRows = numberOfRows;
    }

    public void AddSeatReservation(Guid userId, int numberOfSeats)
    {
        seatReservations.Add(new SeatReservation(Id, userId, numberOfSeats));
    }

}
