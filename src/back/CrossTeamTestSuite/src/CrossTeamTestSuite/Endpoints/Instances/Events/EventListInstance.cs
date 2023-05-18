using CrossTeamTestSuite.Endpoints.Contracts.Common;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Events;

public class EventListInstance: EndpointInstance<EventListRequest, PaginatedResponse<EventDto>>
{
    public override async Task<PaginatedResponse<EventDto>?> HandleEndpointAsync(EventListRequest request)
    {
        return await HttpClient.CallEndpointSuccessAsync<EventListRequest, PaginatedResponse<EventDto>>(request);
    }
}
