using FluentAssertions;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Organizers;

public class OrganizerProfileTests : TestBase
{
    [Fact]
    public async Task Organizer_can_see_its_profile()
    {
        var profile = await OrganizerClient.GetSuccessAsync<OrganizerViewEndpoint, OrganizerViewRequest, OrganizerDto>(new());
        
        profile.Should().BeEquivalentTo(DefaultOrganizer);
    }
}
