using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Event : IAggregateRoot<Guid>
{
    private readonly List<Sector> sectors = new();
    public Guid Id { get; private init; }
    public Guid OrganizerId { get; private init; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public IReadOnlyList<Sector> Sectors => sectors;
    
    private Event() { }
    
    public Event(Guid organizerId, string name, string description, string location, DateTime date, List<SectorData> sectors)
    {
        Id = Guid.NewGuid();
        OrganizerId = organizerId;
        Name = name;
        Description = description;
        Location = location;
        Date = date;
        this.sectors.AddRange(sectors.Select(s => new Sector(Id, s.Name, s.PriceInSmallestUnit, s.NumberOfColumns, s.NumberOfRows)));
    }
    
    public record SectorData(string Name, int PriceInSmallestUnit, int NumberOfColumns, int NumberOfRows);
}
