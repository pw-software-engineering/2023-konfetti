using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Instances.Events;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Event;

public class EventListTest: SingleTest
{
    public override string Name => "Event list test";
    public override async Task ExecuteAsync()
    {
        var eventListRequest = new EventListRequest
        {
            PageNumber = 0,
            PageSize = 10
        };
        var eventListInstance = new EventListInstance();
        eventListInstance.SetToken(CommonTokenType.UserToken);
        
        var response = await eventListInstance.HandleEndpointAsync(eventListRequest);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(1);
        
        var eventDto = response.Items.First();
        var @event = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent;

        eventDto.Should().BeEquivalentTo(@event, options => options.Excluding(e => e!.Date));
        eventDto!.Date.Should().BeCloseTo(@event!.Date, TimeSpan.FromSeconds(1));
    }
}
