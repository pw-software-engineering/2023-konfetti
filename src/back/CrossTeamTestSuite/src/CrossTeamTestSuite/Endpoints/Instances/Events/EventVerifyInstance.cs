using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Data.Repository.Events;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Events;

public class EventVerifyInstance: EndpointInstance<EventVerifyRequest>
{
    public override async Task HandleEndpointAsync(EventVerifyRequest request)
    {
        await HttpClient.CallEndpointSuccessAsync(request);
        DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Status = EventStatus.Verified;
    }
}
