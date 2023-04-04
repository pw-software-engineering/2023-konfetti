using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Events;

public class ManagingEventTests : TestBase
{
    [Fact]
    public async Task Organizer_can_manage_event()
    {
        var eventId = await OrganizerClient.PostSuccessAsync<CreateEventEndpoint, CreateEventRequest, IdResponse>(new()
        {
            Name = "event",
            Description = "description",
            Location = "location",
            Date = DateTime.UtcNow.AddDays(1),
            Sectors = new()
            {
                new()
                {
                    Name = "sector1",
                    PriceInSmallestUnit = 2,
                    NumberOfRows = 2,
                    NumberOfColumns = 4,
                },
            },
        });
        
        // TODO: get event with some request and check data correctness
    }
}
