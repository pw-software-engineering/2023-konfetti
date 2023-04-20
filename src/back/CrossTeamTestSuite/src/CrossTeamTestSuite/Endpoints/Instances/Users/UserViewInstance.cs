using CrossTeamTestSuite.Endpoints.Contracts.Users;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Users;

public class UserViewInstance: EndpointInstance<UserViewRequest, UserDto>
{
    public override async Task<UserDto?> HandleEndpointAsync(UserViewRequest request)
    {
        return await HttpClient.CallEndpointAsync<UserViewRequest, UserDto>(request);
    }
}
