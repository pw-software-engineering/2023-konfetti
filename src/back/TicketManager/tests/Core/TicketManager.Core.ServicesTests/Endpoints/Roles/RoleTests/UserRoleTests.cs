using TicketManager.Core.Services.Endpoints.Users;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class UserRoleTests
{
    private readonly RoleTestsBase testsBase;
    public UserRoleTests()
    {
        testsBase = new RoleTestsBase();
    }

    [Fact]
    public void WhenUserRegisterChecked_ItShouldAllowAnonymous()
    {
        var testInstance = testsBase.GetRoleTestInstance<RegisterUserEndpoint>();
        testInstance.AsAnonymous();
    }
    
    [Fact]
    public void WhenUserViewChecked_ItShouldAllowOnlyUser()
    {
        var testInstance = testsBase.GetRoleTestInstance<UserViewEndpoint>();
        testInstance.AsUser().Check();
    }
}
