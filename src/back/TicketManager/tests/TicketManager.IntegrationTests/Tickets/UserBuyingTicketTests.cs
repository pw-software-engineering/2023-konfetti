using FluentAssertions;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Contracts.Payments;
using TicketManager.Core.Contracts.Tickets;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.Core.Services.Endpoints.Payments;
using TicketManager.Core.Services.Endpoints.Tickets;
using TicketManager.IntegrationTests.Extensions;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.IntegrationTests.Tickets;

public class UserBuyingTicketTests : TestBase
{
    [Fact]
    public async Task User_can_buy_ticket()
    {
        var @event = await CreateEventAsync();
        await ChangeEventStatusAsync(@event.Id);
        var ticketId = await ButTicketAsync(@event);
        var ticket = await GetTicketAsync(ticketId);
        VerifyTicket(ticket, @event);
    }

    private void VerifyTicket(TicketDto ticket, EventDto @event)
    {
        var expected = new TicketDto()
        {
            Id = ticket.Id,
            Event = @event,
            PriceInSmallestUnit = ticket.Seats.Count * @event.Sectors.First(s => s.Name == ticket.SectorName).PriceInSmallestUnit,
            SectorName = "s1",
        };

        ticket.Should().BeEquivalentTo(
            expected,
            options => options
                .Excluding(t => t.Seats)
                .Excluding(t => t.Event.Date));
    }

    private async Task<TicketDto> GetTicketAsync(Guid ticketId)
    {
        return await UserClient.GetSuccessAsync<TicketDetailsEndpoint, TicketDetailsRequest, TicketDto>(new()
        {
            Id = ticketId,
        });
    }

    private async Task<Guid> ButTicketAsync(EventDto @event)
    {
        var payment = await UserClient.PostSuccessAsync<TicketBuyEndpoint, TicketBuyRequest, TicketPaymentDto>(new()
        {
            EventId = @event.Id,
            NumberOfSeats = 5,
            SectorName = "s1",
        });
        
        await PaymentClient.PostSuccessAsync<ConfirmPaymentEndpoint, ConfirmPaymentRequest>(new() { Id = payment.PaymentId });
        
        var finishPayment = await UserClient
            .PostSuccessAsync<FinishPaymentEndpoint, FinishPaymentRequest, FinishPaymentResponse>(new()
            {
                PaymentId = payment.PaymentId,
            });

        await WaitForProcessingAsync();

        return finishPayment.TicketId!.Value;
    }

    private async Task<EventDto> CreateEventAsync()
    {
        var result = new EventDto()
        {
            Name = "name",
            Description = "desc",
            Date = DateTime.UtcNow.AddDays(5),
            Location = "location",
            OrganizerId = DefaultOrganizer.Id,
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfRows = 10,
                    NumberOfColumns = 10,
                    PriceInSmallestUnit = 10,
                },
                new()
                {
                    Name = "s2",
                    NumberOfRows = 20,
                    NumberOfColumns = 20,
                    PriceInSmallestUnit = 20,
                },
            },
        };

        var eventId = await OrganizerClient.PostSuccessAsync<CreateEventEndpoint, CreateEventRequest, IdResponse>(new()
        {
            Name = result.Name,
            Description = result.Description,
            Date = result.Date,
            Location = result.Location,
            Sectors = result.Sectors,
        });
        
        await AdminClient.PostSuccessAsync<EventDecideEndpoint, EventDecideRequest>(new()
        {
            Id = eventId.Id,
            IsAccepted = true,
        });

        result.Id = eventId.Id;
        
        return result;
    }

    private async Task ChangeEventStatusAsync(Guid eventId)
    {

        await OrganizerClient.PostSuccessAsync<EventPublishEndpoint, EventStatusManipulationRequest>(
            new EventStatusManipulationRequest
            {
                Id = eventId
            });
        await OrganizerClient.PostSuccessAsync<EventSaleStartEndpoint, EventSaleStatusRequest>(new EventSaleStatusRequest
        {
            EventId = eventId
        });
    }
}
