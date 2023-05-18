using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Contracts.Common;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Organizers;

public class OrganizerListInstance: EndpointInstance<OrganizerListRequest, PaginatedResponse<OrganizerDto>>
{
    public override async Task<PaginatedResponse<OrganizerDto>?> HandleEndpointAsync(OrganizerListRequest request)
    {
        var response = await HttpClient.CallEndpointSuccessAsync<OrganizerListRequest, PaginatedResponse<OrganizerDto>>(request);
        return response;
    }
}
