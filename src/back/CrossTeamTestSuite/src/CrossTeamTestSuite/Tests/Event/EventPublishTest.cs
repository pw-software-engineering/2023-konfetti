using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Instances.Events;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.Event;

public class EventPublishTest: SingleTest
{
    public override string Name => "Event can be published";
    public override async Task ExecuteAsync()
    {
        var request = new EventPublishRequest
        {
            Id = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Id
        };
        var instance = new EventPublishInstance();
        instance.SetToken(CommonTokenType.OrganizerToken);
        
        await instance.HandleEndpointAsync(request);
    }
}
