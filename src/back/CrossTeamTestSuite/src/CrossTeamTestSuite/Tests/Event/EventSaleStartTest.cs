using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Instances.Events;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.Event;

public class EventSaleStartTest: SingleTest
{
    public override string Name => "Event sale can be started";
    public override async Task ExecuteAsync()
    {
        var request = new EventSaleStartRequest
        {
            EventId = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Id
        };
        
        var instance = new EventSaleStartInstance();
        instance.SetToken(CommonTokenType.OrganizerToken);
        await instance.HandleEndpointAsync(request);
    }
}
