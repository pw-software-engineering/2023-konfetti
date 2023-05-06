using FluentAssertions;
using TicketManager.Core.Domain.Events;
using Xunit;

namespace TicketManager.Core.DomainTests.Events;

public class SectorGetNumberOfFreeSeatsTests
{
    private const int NumberOfColumns = 5;
    private const int NumberOfRows = 5;
    
    private readonly Sector sector = new(Guid.NewGuid(), "name", 10, NumberOfColumns, NumberOfRows);

    [Fact]
    public void WhenNoSeatIsTaken_ItShouldReturnAllSeats()
    {
        var expected = NumberOfRows * NumberOfColumns;

        var actual = sector.GetNumberOfFreeSeats();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void WhenSomeSeatsAreReserved_ItShouldReturnOthers()
    {
        int reservedSeats = 10;
        sector.AddSeatReservation(Guid.NewGuid(), 5, Guid.NewGuid());
        sector.AddSeatReservation(Guid.NewGuid(), 5, Guid.NewGuid());
        var expected = NumberOfRows * NumberOfColumns - reservedSeats;

        var actual = sector.GetNumberOfFreeSeats();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void WhenSomeSeatsAreTaken_ItShouldReturnOthers()
    {
        int takenSeats = 10;
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 5, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 5, paymentId);
        sector.TakeSeats(paymentId);
        var expected = NumberOfRows * NumberOfColumns - takenSeats;

        var actual = sector.GetNumberOfFreeSeats();

        actual.Should().Be(expected);
    }
    
    [Fact]
    public void WhenSomeSeatsAreTakenOrReserved_ItShouldReturnOthers()
    {
        int takenOrReservedSeats = 10;
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 5, paymentId);
        sector.TakeSeats(paymentId);
        sector.AddSeatReservation(Guid.NewGuid(), 5, Guid.NewGuid());
        var expected = NumberOfRows * NumberOfColumns - takenOrReservedSeats;

        var actual = sector.GetNumberOfFreeSeats();

        actual.Should().Be(expected);
    }
}
