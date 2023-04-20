using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Endpoints.Instances.Organizers;

public class OrganizerDecideInstance: EndpointInstance<OrganizerDecideRequest>
{
    public override async Task HandleEndpointAsync(OrganizerDecideRequest request)
    {
        await HttpClient.CallEndpointAsync(request);
    }
}
