using Xunit;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Services.TokenManager;

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
}
