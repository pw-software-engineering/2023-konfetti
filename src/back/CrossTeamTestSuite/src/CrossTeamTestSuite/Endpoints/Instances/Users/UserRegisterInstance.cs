using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Data.Repository.Users;
using CrossTeamTestSuite.Endpoints.Contracts.Users;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Users;

public class UserRegisterInstance: EndpointInstance<UserRegisterRequest>
{
    public override async Task HandleEndpointAsync(UserRegisterRequest request)
    {
        await HttpClient.CallEndpointSuccessAsync(request);
        
        var dataAccess = DataAccessSingleton.GetInstance();
        var user = new User(request.Email, request.Password, request.FirstName, request.LastName, request.BirthDate);
        dataAccess.UserRepository.AddUser(user);
    }
}
