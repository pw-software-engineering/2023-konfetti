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
    public async Task Organizer_can_manage_event()
    {
        await CreateEvent();
    }

    private async Task VerifyEvent()
    {
        //TODO: finish this test
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
}
