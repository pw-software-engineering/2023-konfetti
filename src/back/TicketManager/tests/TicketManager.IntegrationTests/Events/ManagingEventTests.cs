using FluentAssertions;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Events;

public class ManagingEventTests : TestBase
{
    private Event defaultEvent;
    private List<Sector> sectors;
    private Guid realEventId;
    public ManagingEventTests()
    {
        defaultEvent = new Event(DefaultOrganizer.Id, "event", "description", "location", DateTime.UtcNow.AddDays(1));
        sectors = new List<Sector>
        {
            new Sector(defaultEvent.Id,"sector1", 2, 2, 4),
            new Sector(defaultEvent.Id,"sector2", 2, 2, 4),
        };
    }
    [Fact]
    public async Task Organizer_can_manage_event_and_recall_it()
    {
        await CreateEvent();
        await VerifyEvent();
        
        await VerifyEventPositively();

        await PublishEvent();
        await VerifyEvent();

        await HoldEvent();
        await VerifyEvent();
        
        await RecallEvent();
        await VerifyEvent();
    }
    
    [Fact]
    public async Task Organizer_can_manage_event_and_start_stop_sale()
    {
        await CreateEvent();
        await VerifyEvent();

        await VerifyEventPositively();

        await PublishEvent();
        await VerifyEvent();

        await SaleStart();
        await VerifyEvent();
        
        await SaleStop();
        await VerifyEvent();
        
        await SaleStart();
        await VerifyEvent();
    }

    private async Task PublishEvent()
    {
        await OrganizerClient.PostSuccessAsync<EventPublishEndpoint, EventStatusManipulationRequest>(new()
        {
            Id = realEventId,
        });
        defaultEvent.ChangeEventStatus(EventStatus.Published);
    }
    
    private async Task RecallEvent()
    {
        await OrganizerClient.PostSuccessAsync<EventRecallEndpoint, EventStatusManipulationRequest>(new()
        {
            Id = realEventId,
        });
        defaultEvent.ChangeEventStatus(EventStatus.Recalled);
    }
    
    private async Task HoldEvent()
    {
        await OrganizerClient.PostSuccessAsync<EventHoldEndpoint, EventStatusManipulationRequest>(new()
        {
            Id = realEventId,
        });
        defaultEvent.ChangeEventStatus(EventStatus.Held);
    }
    
    private async Task SaleStart()
    {
        await OrganizerClient.PostSuccessAsync<EventSaleStartEndpoint, EventSaleStatusRequest>(new()
        {
            EventId = realEventId,
        });
        defaultEvent.ChangeEventStatus(EventStatus.Opened);
    }
    
    private async Task SaleStop()
    {
        await OrganizerClient.PostSuccessAsync<EventSaleStopEndpoint, EventSaleStatusRequest>(new()
        {
            EventId = realEventId,
        });
        defaultEvent.ChangeEventStatus(EventStatus.Closed);
    }
    
    private async Task VerifyEvent()
    {
        var @event = await UserClient.GetSuccessAsync<GetEventEndpoint, GetEventRequest, EventDto>(new GetEventRequest
        {
            EventId = realEventId
        });
        @event.Should().BeEquivalentTo(defaultEvent, options => options
            .Excluding(e => e.DateModified)
            .Excluding(e => e.Id)
            .Excluding(e => e.IsDeleted)
            .Using<DateTime>(ctx => 
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, precision: TimeSpan.FromSeconds(1))).WhenTypeIs<DateTime>());
    }

    private async Task CreateEvent()
    {
        var eventId = await OrganizerClient.PostSuccessAsync<CreateEventEndpoint, CreateEventRequest, IdResponse>(new()
        {
            Name = defaultEvent.Name,
            Description = defaultEvent.Description,
            Location = defaultEvent.Location,
            Date = defaultEvent.Date,
            Sectors = sectors.Select(s => new SectorDto
            {
                Name = s.Name,
                PriceInSmallestUnit = s.PriceInSmallestUnit,
                NumberOfColumns = s.NumberOfColumns,
                NumberOfRows = s.NumberOfRows,
            }).ToList()
        });

        realEventId = eventId.Id;
    }

    private async Task VerifyEventPositively()
    {
        await AdminClient.PostSuccessAsync<EventDecideEndpoint, EventDecideRequest>(new()
        {
            Id = realEventId,
            IsAccepted = true,
        });

        defaultEvent.ChangeEventStatus(EventStatus.Verified);
    }
}
