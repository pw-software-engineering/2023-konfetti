using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Events;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints;

public class RoleTests
{
    [Fact]
    public void Test()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var dbContext = dbContextMock.Object;
        var eventsMock = new Mock<Repository<Event, Guid>>(dbContext);
        var events = eventsMock.Object;
            
        var endpoint = Factory.Create<CreateEventEndpoint>();

        endpoint.Configure();
        var roles = endpoint.Definition.AllowedRoles;
        roles.Should().NotBeNull();
        roles.Count().Should().Be(1);
    }
}
