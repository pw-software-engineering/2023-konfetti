using FluentAssertions;
using TicketManager.Core.Domain.Events;
using Xunit;

namespace TicketManager.Core.DomainTests.Events;

public class SectorTakeSeatsTests
{
    private const int NumberOfColumns = 5;
    private const int NumberOfRows = 5;
    
    private readonly Sector sector = new(Guid.NewGuid(), "name", 10, NumberOfColumns, NumberOfRows);

    [Fact]
    public void WhenSectorIsFree_ItShouldTakeFirstSeats()
    {
        var expected = new List<TakenSeat>()
        {
            new(0, 0),
            new(0, 1),
            new(0, 2),
        };
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);

        var actual = sector.TakeSeats(paymentId);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void WhenFirstRowHasEnoughSeats_ItShouldTakeSeatsInFirstRow()
    {
        var expected = new List<TakenSeat>()
        {
            new(0, 3),
            new(0, 4),
        };
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 2, paymentId);

        var actual = sector.TakeSeats(paymentId);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void WhenFirstRowDoesNotHaveEnoughSeatsButSecondRowIsEmpty_ItShouldTakeSeatsInSecondRow()
    {
        var expected = new List<TakenSeat>()
        {
            new(1, 0),
            new(1, 1),
            new(1, 2),
        };
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);

        var actual = sector.TakeSeats(paymentId);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void WhenFirstRowDoesNotHaveEnoughSeatsButSecondRowHasEnoughSeats_ItShouldTakeSeatsInSecondRow()
    {
        var expected = new List<TakenSeat>()
        {
            new(1, 2),
            new(1, 3),
            new(1, 4),
        };
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 4, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 2, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);

        var actual = sector.TakeSeats(paymentId);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void WhenNoRowHasEnoughSeats_ItShouldTakeSeatsFromCoupleDifferentRows()
    {
        var expected = new List<TakenSeat>()
        {
            new(0, 3),
            new(0, 4),
            new(1, 3),
            new(1, 4),
            new(2, 3),
        };
        var paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 3, paymentId);
        sector.TakeSeats(paymentId);
        paymentId = Guid.NewGuid();
        sector.AddSeatReservation(Guid.NewGuid(), 5, paymentId);

        var actual = sector.TakeSeats(paymentId);

        actual.Should().BeEquivalentTo(expected);
    }
}
