using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Organizers;

public class OrganizerViewInstance: EndpointInstance<OrganizerViewRequest, OrganizerDto>
{
    public override async Task<OrganizerDto?> HandleEndpointAsync(OrganizerViewRequest request)
    {
        return await HttpClient.CallEndpointAsync<OrganizerViewRequest, OrganizerDto>(request);
    }
}
