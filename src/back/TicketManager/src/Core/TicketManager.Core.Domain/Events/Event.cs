using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Event : IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public Guid OrganizerId { get; private init; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public EventStatus Status { get; private set; }
    
    public DateTime DateModified { get; set; }
    
    private Event() { }
    
    public Event(Guid organizerId, string name, string description, string location, DateTime date)
    {
        Id = Guid.NewGuid();
        OrganizerId = organizerId;
        Name = name;
        Description = description;
        Location = location;
        Date = date;
    }
}
