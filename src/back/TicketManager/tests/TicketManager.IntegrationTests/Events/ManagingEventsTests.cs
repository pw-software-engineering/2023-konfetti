using FluentAssertions;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.IntegrationTests.Extensions;
using TicketManager.IntegrationTests.Helpers;
using Xunit;

namespace TicketManager.IntegrationTests.Events;

public class ManagingEventsTests : TestBase
{
    private readonly List<EventDto> events = new();

    [Fact]
    public async Task Organizer_can_manage_events()
    {
        await CreateEventAsync(new()
        {
            Name = "name 1",
            Date = TimeProvider.Now().AddDays(5),
            Description = "desc",
            Location = "location",
            Status = EventStatusDto.Verified,
            OrganizerId = DefaultOrganizer.Id,
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfColumns = 2,
                    NumberOfRows = 5,
                    PriceInSmallestUnit = 4,
                },
                new()
                {
                    Name = "s2",
                    NumberOfColumns = 2,
                    NumberOfRows = 5,
                    PriceInSmallestUnit = 4,
                },
            },
        });
        await VerifyEventsAsync();
        
        await CreateEventAsync(new()
        {
            Name = "name 2",
            Date = TimeProvider.Now().AddDays(5),
            Description = "desc",
            Location = "location",
            Status = EventStatusDto.Verified,
            OrganizerId = DefaultOrganizer.Id,
            Sectors = new()
            {
                new()
                {
                    Name = "s1",
                    NumberOfColumns = 2,
                    NumberOfRows = 5,
                    PriceInSmallestUnit = 4,
                },
                new()
                {
                    Name = "s2",
                    NumberOfColumns = 2,
                    NumberOfRows = 5,
                    PriceInSmallestUnit = 4,
                },
            },
        });
        await VerifyEventsAsync();

        await DeleteEventAsync(events[0].Id);
        await VerifyEventsAsync();
    }

    private async Task CreateEventAsync(EventDto @event)
    {
        var response = await OrganizerClient.PostSuccessAsync<CreateEventEndpoint, CreateEventRequest, IdResponse>(new()
        {
            Name = @event.Name,
            Description = @event.Description,
            Location = @event.Location,
            Date = @event.Date,
            Sectors = @event.Sectors,
        });

        @event.Id = response.Id;

        events.Add(@event);
    }

    private async Task VerifyEventsAsync()
    {
        var actual = await OrganizerClient
            .GetSuccessAsync<ListEventsForOrganizerEndpoint, ListEventForOrganizerRequest, PaginatedResponse<EventDto>>(
                new()
                {
                    PageNumber = 0,
                    PageSize = 100,
                });

        actual.Items.Should().BeEquivalentTo(events);
    }

    private async Task DeleteEventAsync(Guid id)
    {
        await OrganizerClient.PostSuccessAsync<DeleteEventEndpoint, DeleteEventRequest>(new() { Id = id });

        events.RemoveAll(e => e.Id == id);
    }
}
