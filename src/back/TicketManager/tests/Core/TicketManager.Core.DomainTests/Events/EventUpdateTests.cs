using FluentAssertions;
using TicketManager.Core.Domain.Events;
using Xunit;

namespace TicketManager.Core.DomainTests.Events;

public class EventUpdateTests
{
    private const int sectorPriceInSmallestUnit = 10;
    private const int sectorNumberOfColumns = 5;
    private const int sectorNumberOfRows = 5;
    
    private readonly Event @event = new(Guid.NewGuid(), "name", "description", "location", DateTime.Now.AddDays(1));
    private readonly Sector sector = new(Guid.NewGuid(), "name", sectorPriceInSmallestUnit, sectorNumberOfColumns, sectorNumberOfRows);
    
    [Fact]
    public void WhenEventNameIsUpdated_ItShouldBeChanged()
    {
        var newName = "new name";

        @event.UpdateName(newName);

        @event.Name.Should().Be(newName);
    } 
    
    [Fact]
    public void WhenEventDescriptionIsUpdated_ItShouldBeChanged()
    {
        var newDescription = "new description";

        @event.UpdateDescription(newDescription);

        @event.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void WhenEventLocationIsUpdated_ItShouldBeChanged()
    {
        var newLocation = "new location";

        @event.UpdateLocation(newLocation);

        @event.Location.Should().Be(newLocation);
    }
    
    [Fact]
    public void WhenSectorPriceIsUpdated_ItShouldBeChanged()
    {
        var newPrice = 20;

        sector.Update(newPrice, sectorNumberOfColumns, sectorNumberOfRows);

        sector.PriceInSmallestUnit.Should().Be(newPrice);
    }
    
    [Fact]
    public void WhenSectorNumberOfColumnsIsUpdated_ItShouldBeChanged()
    {
        var newNumberOfColumns = 10;

        sector.Update(sectorPriceInSmallestUnit, newNumberOfColumns, sectorNumberOfRows);

        sector.NumberOfColumns.Should().Be(newNumberOfColumns);
    }
    
    [Fact]
    public void WhenSectorNumberOfRowsIsUpdated_ItShouldBeChanged()
    {
        var newNumberOfRows = 10;

        sector.Update(sectorPriceInSmallestUnit, sectorNumberOfColumns, newNumberOfRows);

        sector.NumberOfRows.Should().Be(newNumberOfRows);
    }
    
    [Fact]
    public void WhenSectorIsUpdatedWithSameValues_ItShouldReturnFalse()
    {
        var result = sector.Update(sectorPriceInSmallestUnit, sectorNumberOfColumns, sectorNumberOfRows);

        result.Should().BeFalse();
    }
}
