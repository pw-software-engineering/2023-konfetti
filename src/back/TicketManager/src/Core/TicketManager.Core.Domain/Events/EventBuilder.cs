namespace TicketManager.Core.Domain.Events;

public class EventBuilder
{
    private Guid id;
    private string name = "";
    private string description = "";
    private string location = "";
    private DateTime date;
    private List<SectorData> sectors = new();

    public EventBuilder WithGeneratedId()
    {
        id = Guid.NewGuid();
        return this;
    }

    public EventBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    public EventBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    public EventBuilder WithLocation(string location)
    {
        this.location = location;
        return this;
    }

    public EventBuilder WithDate(DateTime date)
    {
        this.date = date;
        return this;
    }

    public EventBuilder WithSectors(List<SectorData> sectors)
    {
        this.sectors = sectors;
        return this;
    }

    public Event Build()
    {
        Validate();
        return new Event(
            id,
            name,
            description,
            location,
            date,
            sectors
                .Select(s => new Sector(id, s.Name, s.PriceInSmallestUnit, s.NumberOfColumns, s.NumberOfRows))
                .ToList());
    }

    private void Validate()
    {
        ValidateName();
        ValidateDescription();
        ValidateLocation();
        ValidateSectors();
    }

    private void ValidateSectors()
    {
        if (sectors.Count == 0)
        {
            throw new ArgumentException("At least one sector is required");
        }

        if (sectors.Any(s => string.IsNullOrWhiteSpace(s.Name)))
        {
            throw new ArgumentException("Sectors names are required");
        }

        if (sectors.Any(s => s.PriceInSmallestUnit <= 0))
        {
            throw new ArgumentException("Price has to be positive");
        }

        if (sectors.Any(s => s.NumberOfColumns <= 0))
        {
            throw new ArgumentException("Number of columns has to be positive");
        }

        if (sectors.Any(s => s.NumberOfRows <= 0))
        {
            throw new ArgumentException("Number of rows has to be positive");
        }
    }

    private void ValidateLocation()
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location is required");
        }
    }

    private void ValidateDescription()
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required");
        }
    }

    private void ValidateName()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required");
        }
    }

    public record SectorData(string Name, int PriceInSmallestUnit, int NumberOfColumns, int NumberOfRows);
}