using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Instances;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Admin;

public class AdminLoginTest: SingleTest
{
    private string adminMail;
    private string adminPassword;
    public override string Name => "Admin login test";

    public AdminLoginTest(string adminMail, string adminPassword)
    {
        this.adminMail = adminMail;
        this.adminPassword = adminPassword;
    }
    public override async Task ExecuteAsync()
    {
        var adminLoginRequest = new AccountLoginRequest()
        {
            Email = adminMail,
            Password = adminPassword,
        };
        var accountLoginInstance = new AccountLoginInstance();
        var response = await accountLoginInstance.HandleEndpointAsync(adminLoginRequest);
        response.AccessToken.Should().NotBeNullOrEmpty();

        CommonTokenSingleton.SetToken(CommonTokenType.AdminToken, response.AccessToken);
    }
}
