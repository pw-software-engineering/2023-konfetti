using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Data.Repository.Events;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Events;

public class EventPublishInstance: EndpointInstance<EventPublishRequest>
{
    public override async Task HandleEndpointAsync(EventPublishRequest request)
    {
        await HttpClient.CallEndpointSuccessAsync(request);
        DataAccessSingleton.GetInstance().EventRepository.DefaultEvent!.Status = EventStatus.Published;
    }
}
