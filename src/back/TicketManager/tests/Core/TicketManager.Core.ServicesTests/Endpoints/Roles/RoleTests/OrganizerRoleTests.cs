using TicketManager.Core.Services.Endpoints.Organizers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class OrganizerRoleTests
{
    private readonly RoleTestsBase testsBase;
    public OrganizerRoleTests()
    {
        testsBase = new RoleTestsBase();
    }

    [Fact]
    public void WhenOrganizerRegisterChecked_ItShouldAllowAnonymous()
    {
        var testInstance = testsBase.GetRoleTestInstance<RegisterOrganizerEndpoint>();
        testInstance.AsAnonymous();
    }
    
    [Fact]
    public void WhenOrganizerViewChecked_ItShouldAllowOnlyOrganizer()
    {
        var testInstance = testsBase.GetRoleTestInstance<OrganizerViewEndpoint>();
        testInstance.AsOrganizer().Check();
    }
    
    [Fact]
    public void WhenOrganizerDecideChecked_ItShouldAllowOnlyAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<OrganizerDecideEndpoint>();
        testInstance.AsAdmin().Check();
    }
}
