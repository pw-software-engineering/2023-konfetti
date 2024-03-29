using TicketManager.Core.Services.Endpoints.Users;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class UserRoleTests
{
    private readonly RoleTestsBase testsBase = new();

    [Fact]
    public void WhenUserRegisterChecked_ItShouldAllowAnonymous()
    {
        var testInstance = testsBase.GetRoleTestInstance<RegisterUserEndpoint>();
        testInstance.CheckAnonymous();
    }
    
    [Fact]
    public void WhenUserViewChecked_ItShouldAllowOnlyUser()
    {
        var testInstance = testsBase.GetRoleTestInstance<UserViewEndpoint>();
        testInstance.AsUser().Check();
    }
}
