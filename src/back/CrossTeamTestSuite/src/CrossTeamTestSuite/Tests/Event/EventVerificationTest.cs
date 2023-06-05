using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Instances.Events;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.Event;

public class EventVerificationTest: SingleTest
{
    public override string Name => "Event can be verified";
    public override async Task ExecuteAsync()
    {
        var request = new EventVerifyRequest
        {
            Id = DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Id,
            IsAccepted = true
        };
        var instance = new EventVerifyInstance();
        instance.SetToken(CommonTokenType.AdminToken);

        await instance.HandleEndpointAsync(request);
    }
}
