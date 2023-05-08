using FluentAssertions;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.IntegrationTests.Extensions;
using TicketManager.IntegrationTests.Helpers;
using Xunit;

namespace TicketManager.IntegrationTests.Events;

public class UserInteractingWithEventTests : TestBase
{
    [Fact]
    public async Task User_can_see_events()
    {
        var event1 = new EventDto
        {
            Name = "event 1",
            Description = "description",
            Date = TimeProvider.Now().AddDays(1),
            Location = "location",
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfRows = 1,
                    NumberOfColumns = 1,
                    PriceInSmallestUnit = 1,
                },
            },
        };
        await AddEventAsync(event1);
        await VerifyEventsAsync(event1);
        
        var event2 = new EventDto
        {
            Name = "event 2",
            Description = "description",
            Date = TimeProvider.Now().AddDays(3),
            Location = "location2",
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfRows = 2,
                    NumberOfColumns = 3,
                    PriceInSmallestUnit = 123,
                },
                new()
                {
                    Name = "s2",
                    NumberOfRows = 2,
                    NumberOfColumns = 3,
                    PriceInSmallestUnit = 123,
                },
                new()
                {
                    Name = "s3",
                    NumberOfRows = 2,
                    NumberOfColumns = 3,
                    PriceInSmallestUnit = 123,
                },
            },
        };
        await AddEventAsync(event2);
        await VerifyEventsAsync(event1, event2);
        
        var event3 = new EventDto
        {
            Name = "event 3",
            Description = "description",
            Date = TimeProvider.Now().AddDays(1),
            Location = "location",
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfRows = 1,
                    NumberOfColumns = 1,
                    PriceInSmallestUnit = 1,
                },
            },
        };
        await AddEventAsync(event3);
        await VerifyEventsAsync(event1, event2, event3);
    }

    private async Task AddEventAsync(EventDto @event)
    {
        var eventId = await OrganizerClient.PostSuccessAsync<CreateEventEndpoint, CreateEventRequest, IdResponse>(new()
        {
            Name = @event.Name,
            Description = @event.Description,
            Location = @event.Location,
            Date = @event.Date,
            Sectors = @event.Sectors,
        });

        @event.Id = eventId.Id;
        @event.OrganizerId = DefaultOrganizer.Id;
    }

    private async Task VerifyEventsAsync(params EventDto[] events)
    {
        var actual = await UserClient.GetSuccessAsync<ListEventsEndpoint, ListEventsRequest, PaginatedResponse<EventDto>>(new()
        {
            PageNumber = 0,
            PageSize = 10,
        });

        actual.Items.Should().BeEquivalentTo(events, options => options.Excluding(e => e.Status));
    }
}
