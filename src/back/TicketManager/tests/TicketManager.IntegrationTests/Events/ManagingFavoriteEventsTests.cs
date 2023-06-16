using FluentAssertions;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Events;

public class ManagingFavoriteEventsTests : TestBase
{
    private readonly List<Guid> favoriteEvents = new();
    
    [Fact]
    public async Task User_can_manage_his_favorite_events()
    {
        var event1 = await CreateEventAsync();
        var event2 = await CreateEventAsync();
        var event3 = await CreateEventAsync();
        var event4 = await CreateEventAsync();

        await AddEventToFavoritesAsync(event1);
        await VerifyFavoriteEventsAsync();
        
        await AddEventToFavoritesAsync(event2);
        await VerifyFavoriteEventsAsync();
        
        await AddEventToFavoritesAsync(event3); 
        await VerifyFavoriteEventsAsync();
        
        await RemoveEventFromFavoritesAsync(event2); 
        await VerifyFavoriteEventsAsync();
        
        await AddEventToFavoritesAsync(event4); 
        await VerifyFavoriteEventsAsync();
        
        await RemoveEventFromFavoritesAsync(event3); 
        await VerifyFavoriteEventsAsync();
        
        await RemoveEventFromFavoritesAsync(event4); 
        await VerifyFavoriteEventsAsync();
        
        await AddEventToFavoritesAsync(event2);
        await VerifyFavoriteEventsAsync();
    }

    private async Task AddEventToFavoritesAsync(EventDto @event)
    {
        favoriteEvents.Add(@event.Id);

        await UserClient.PostSuccessAsync<AddEventToFavoritesEndpoint, AddEventToFavoritesRequest>(new() { EventId = @event.Id, });
    }
    
    private async Task RemoveEventFromFavoritesAsync(EventDto @event)
    {
        favoriteEvents.Remove(@event.Id);

        await UserClient.PostSuccessAsync<DeleteEventFromFavoritesEndpoint, DeleteEventFromFavoritesRequest>(new() { EventId = @event.Id, });
    }

    private async Task VerifyFavoriteEventsAsync()
    {
        var actual = await UserClient.GetSuccessAsync<MyFavoriteEventsEndpoint, MyFavoriteEventsRequest, List<EventDto>>(new());

        actual.Select(e => e.Id).Should().BeEquivalentTo(favoriteEvents);
    }
    
    private async Task<EventDto> CreateEventAsync()
    {
        var result = new EventDto()
        {
            Name = Guid.NewGuid().ToString(),
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
}
