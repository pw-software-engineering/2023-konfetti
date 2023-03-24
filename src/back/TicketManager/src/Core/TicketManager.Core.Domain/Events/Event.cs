using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Event : IAggregateRoot<Guid>
{
    private readonly List<Sector> sectors = new();
    public Guid Id { get; private init; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public IReadOnlyList<Sector> Sectors => sectors;
    
    public Event(string name, string description, string location, DateTime date, List<Sector> sectors)
    {
        Name = name;
        Description = description;
        Location = location;
        Date = date;
        this.sectors.AddRange(sectors);
    }
}
