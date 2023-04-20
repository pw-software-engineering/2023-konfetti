using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Instances.Accounts;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Organizer;

public class OrganizerLoginTest: SingleTest
{
    public override string Name => "Organizer login test";

    public override async Task ExecuteAsync()
    {
        var dataAccess = DataAccessSingleton.GetInstance();
        var organizerLoginRequest = new AccountLoginRequest
        {
            Email = dataAccess.OrganizerRepository.DefaultEmail,
            Password = dataAccess.OrganizerRepository.DefaultPassword
        };

        var accountLoginInstance = new AccountLoginInstance();
        var response = await accountLoginInstance.HandleEndpointAsync(organizerLoginRequest);

        response.Should().NotBeNull();
        response!.AccessToken.Should().NotBeNullOrEmpty();
        
        CommonTokenSingleton.SetToken(CommonTokenType.OrganizerToken, response.AccessToken);
    }
}
