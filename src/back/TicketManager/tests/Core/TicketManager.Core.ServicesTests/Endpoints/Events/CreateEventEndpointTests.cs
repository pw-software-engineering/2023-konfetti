using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Events;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Events;

public class CreateEventEndpointTests
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldAddEvent()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var dbContext = dbContextMock.Object;
        var eventsMock = new Mock<Repository<Event, Guid>>(dbContext);
        var events = eventsMock.Object;
        var endpoint = Factory.Create<CreateEventEndpoint>(events);
        var req = new CreateEventRequest
        {
            AccountId = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Location = "location",
            Date = DateTime.UtcNow.AddDays(1),
            Sectors = new()
            {
                new()
                {
                    Name = "name",
                    PriceInSmallestUnit = 1,
                    NumberOfColumns = 1,
                    NumberOfRows = 1,
                },
            },
        };
        await endpoint.HandleAsync(req, default);
        
        eventsMock.Verify(e => e.Add(It.IsAny<Event>()), Times.Once);
    }
}
