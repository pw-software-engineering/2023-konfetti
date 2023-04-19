using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Users;
using CrossTeamTestSuite.Endpoints.Instances.Users;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.User;

public class UserRegisterTest: SingleTest
{
    public override string Name => "User register test";

    public override async Task ExecuteAsync()
    {
        var dataAccess = DataAccessSingleton.GetInstance();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = dataAccess.UserRepository.DefaultEmail,
            Password = dataAccess.UserRepository.DefaultPassword,
            FirstName = "James",
            LastName = "Smith",
            BirthDate = new DateOnly(1990, 1, 1)
        };
        
        var userRegisterInstance = new UserRegisterInstance();
        await userRegisterInstance.HandleEndpointAsync(userRegisterRequest);
    }
}
