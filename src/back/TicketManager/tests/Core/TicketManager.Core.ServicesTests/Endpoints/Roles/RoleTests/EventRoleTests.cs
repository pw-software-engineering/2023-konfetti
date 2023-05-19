using TicketManager.Core.Services.Endpoints.Events;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Roles.RoleTests;

public class EventRoleTests
{
    private readonly RoleTestsBase testsBase = new();

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
    
    [Fact]
    public void WhenListEventForOrganizerChecked_ItShouldAllowOnlyOrganizer()
    {
        var testInstance = testsBase.GetRoleTestInstance<ListEventsForOrganizerEndpoint>();
        testInstance.AsOrganizer().Check();
    }
    
    [Fact]
    public void WhenEventHoldChecked_ItShouldAllowOrganizerAndAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventHoldEndpoint>();
        testInstance.AsOrganizer().AsAdmin().Check();
    }
    [Fact]
    public void WhenEventPublishChecked_ItShouldAllowOrganizerAndAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventPublishEndpoint>();
        testInstance.AsOrganizer().AsAdmin().Check();
    }
    [Fact]
    public void WhenEventRecallChecked_ItShouldAllowOrganizerAndAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventRecallEndpoint>();
        testInstance.AsOrganizer().AsAdmin().Check();
    }
    
    [Fact]
    public void WhenEventRestartChecked_ItShouldAllowOrganizerAndAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventRestartEndpoint>();
        testInstance.AsOrganizer().AsAdmin().Check();
    }

    [Fact]
    public void WhenEventSaleStartChecked_ItShouldAllowOnlyOrganizer()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventSaleStartEndpoint>();
        testInstance.AsOrganizer().Check();
    }
    [Fact]
    public void WhenEventSaleStopChecked_ItShouldAllowOnlyOrganizer()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventSaleStopEndpoint>();
        testInstance.AsOrganizer().Check();
    }

    [Fact]
    public void WhenEventUpdateChecked_ItShouldAllowOrganizerAndAdmin()
    {
        var testInstance = testsBase.GetRoleTestInstance<EventUpdateEndpoint>();
        testInstance.AsOrganizer().AsAdmin().Check();
    }
}
