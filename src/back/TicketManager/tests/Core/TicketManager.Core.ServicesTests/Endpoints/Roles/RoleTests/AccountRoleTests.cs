using Xunit;
using TicketManager.Core.Services.Endpoints.Accounts;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class AccountRoleTests
{
    private readonly RoleTestsBase testsBase = new();

    [Fact]
    public void WhenAccountLoginChecked_ItShouldAllowAnonymous()
    {
        var testInstance = testsBase.GetRoleTestInstance<AccountLoginEndpoint>();
        testInstance.CheckAnonymous();
    }

    [Fact]
    public void WhenAccountUpdatePasswordChecked_ItShouldAllowAllRoles()
    {
        var testInstance = testsBase.GetRoleTestInstance<AccountUpdatePasswordEndpoint>();
        testInstance.AsAnyRole().Check();
    }
}
