using FluentAssertions;
using TicketManager.Core.Domain.Events;
using Xunit;

namespace TicketManager.Core.DomainTests.Events;

public class EventBuilderTests
{
    private readonly List<EventBuilder.SectorData> sectors;
    private readonly Guid organizerId = Guid.NewGuid();
    private readonly string name = "name";
    private readonly string description = "description";
    private readonly string location = "location";
    private readonly DateTime date = DateTime.UtcNow.AddDays(1);

    public EventBuilderTests()
    {
        sectors = new()
        {
            new("name 1", 5, 10, 12),
            new("name 2", 10, 1, 1),
            new("name 3", 123, 1123, 123),
            new("name 4", 4, 13, 3),
        };
    }

    [Fact]
    public void WhenCorrectDataIsProvided_ItShouldReturnEvent()
    {
        var @event = new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(sectors)
            .Build();

        @event.OrganizerId.Should().Be(organizerId);
        @event.Name.Should().Be(name);
        @event.Description.Should().Be(description);
        @event.Location.Should().Be(location);
        @event.Date.Should().Be(date);
        @event.Sectors.Should().BeEquivalentTo(sectors);
        @event.Sectors.Should().Match(s => s.All(s => s.EventId == @event.Id));
    }
    
    [Fact]
    public void WhenOrganizerIdIsNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenNameIsNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenDescriptionIsNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenLocationIsNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithDate(date)
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenDateIsNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenPastDateIsProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(DateTime.UtcNow.AddDays(-1))
            .WithSectors(sectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorsAreNotProvided_ItShouldThrowArgumentException()
    {
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorsWithTheSameNameAreProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>
        {
            new("name", 1, 1, 1),
            new("name", 2, 2, 2),
            new("name", 3, 3, 3),
        };
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorWithNegativePriceIsProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>
        {
            new("name1", -1, 1, 1),
            new("name2", 2, 2, 2),
            new("name3", 3, 3, 3),
        };
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorWithNegativeNumberOfRowsIsProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>
        {
            new("name1", 1, 1, -1),
            new("name2", 2, 2, 2),
            new("name3", 3, 3, 3),
        };
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorWithNegativeNumberOfColumnsIsProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>
        {
            new("name1", 1, 1, -1),
            new("name2", 2, 2, 2),
            new("name3", 3, 3, 3),
        };
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenSectorWithEmptyNameIsProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>
        {
            new("", 1, 1, 1),
            new("name2", 2, 2, 2),
            new("name3", 3, 3, 3),
        };
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void WhenEmptySectorListIsProvided_ItShouldThrowArgumentException()
    {
        var invalidSectors = new List<EventBuilder.SectorData>();
        
        var func = () => new EventBuilder()
            .WithGeneratedId()
            .WithOrganizerId(organizerId)
            .WithName(name)
            .WithDescription(description)
            .WithLocation(location)
            .WithDate(date)
            .WithSectors(invalidSectors)
            .Build();

        func.Should().Throw<ArgumentException>();
    }
}
