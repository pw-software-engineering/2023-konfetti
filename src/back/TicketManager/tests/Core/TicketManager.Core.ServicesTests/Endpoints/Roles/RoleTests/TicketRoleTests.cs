using TicketManager.Core.Services.Endpoints.Tickets;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class TicketRoleTests
{
    private readonly RoleTestsBase testsBase = new();

    [Fact]
    public void WhenTicketBuyChecked_ItShouldAllowOnlyUser()
    {
        var testInstance = testsBase.GetRoleTestInstance<TicketBuyEndpoint>();
        testInstance.AsUser().Check();
    }
}
