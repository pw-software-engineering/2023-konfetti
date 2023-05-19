using TicketManager.Core.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class PaymentRoleTests
{
    private readonly RoleTestsBase testsBase = new();

    [Fact]
    public void WhenFinishPaymentChecked_ItShouldAllowOnlyUser()
    {
        var testInstance = testsBase.GetRoleTestInstance<FinishPaymentEndpoint>();
        testInstance.AsUser().Check();
    }
}
