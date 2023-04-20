using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Users;
using CrossTeamTestSuite.Endpoints.Instances.Users;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.User;

public class UserViewTest: SingleTest
{
    public override string Name => "User view test";
    public override async Task ExecuteAsync()
    {
        var userViewRequest = new UserViewRequest();
        var userViewInstance = new UserViewInstance();
        userViewInstance.SetToken(CommonTokenType.UserToken);
        
        var userDto = await userViewInstance.HandleEndpointAsync(userViewRequest);
        userDto.Should().NotBeNull();

        var dataAccess = DataAccessSingleton.GetInstance();
        var user = dataAccess.UserRepository.DefaultAccount;

        userDto.Should().BeEquivalentTo(user, options => options.Excluding(u => u!.Password));
    }
}
