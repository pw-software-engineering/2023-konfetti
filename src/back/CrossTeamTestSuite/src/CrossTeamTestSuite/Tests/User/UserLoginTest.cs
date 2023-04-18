using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Instances;
using CrossTeamTestSuite.Endpoints.Instances.Accounts;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.User;

public class UserLoginTest: SingleTest
{
    public override string Name => "User login test";
    public override async Task ExecuteAsync()
    {
        var dataAccess = DataAccessSingleton.GetInstance();
        
        var userLoginRequest = new AccountLoginRequest
        {
            Email = dataAccess.UserRepository.DefaultEmail,
            Password = dataAccess.UserRepository.DefaultPassword
        };
        
        var accountLoginInstance = new AccountLoginInstance();
        var response = await accountLoginInstance.HandleEndpointAsync(userLoginRequest);
        
        response.Should().NotBeNull();
        response!.AccessToken.Should().NotBeNullOrEmpty();
        
        CommonTokenSingleton.SetToken(CommonTokenType.UserToken, response.AccessToken);
    }
}
