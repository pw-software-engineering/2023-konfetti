using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Tickets;

public class Ticket : IAggregateRoot<Guid>, IOptimisticConcurrent
{
    private readonly List<TicketSeat> seats = new();

    public Guid Id { get; private init; }
    public Guid EventId { get; private init; }
    public Guid SectorId { get; private init; }
    public bool IsPdfGenerated { get; private set; }
    public IReadOnlyList<TicketSeat> Seats => seats;

    public DateTime DateModified { get; set; }
    
    private Ticket() 
    { }
    
    public Ticket(Guid eventId, Guid sectorId, IEnumerable<TicketSeat> seats)
    {
        EventId = eventId;
        SectorId = sectorId;
        this.seats.AddRange(seats);
    }
}
