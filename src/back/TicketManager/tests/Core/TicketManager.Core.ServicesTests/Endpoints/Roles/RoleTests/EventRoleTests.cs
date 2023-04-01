using TicketManager.Core.Services.Endpoints.Events;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class EventRoleTests
{
    private readonly RoleTestsBase testsBase;
    
    public EventRoleTests()
    {
        testsBase = new RoleTestsBase();
    }

    [Fact]
    public void WhenCreateEventChecked_ItShouldAllowOnlyOrganizer()
    {
        var testInstance = testsBase.GetRoleTestInstance<CreateEventEndpoint>();
        testInstance.AsOrganizer().Check();
    }
    
    [Fact]
    public void WhenListEventChecked_ItShouldAllowAllRoles()
    {
        var testInstance = testsBase.GetRoleTestInstance<ListEventsEndpoint>();
        testInstance.AsAnyRole().Check();
    }
}
