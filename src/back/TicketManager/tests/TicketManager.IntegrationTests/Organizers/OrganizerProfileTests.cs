using System.Net;
using FastEndpoints;
using FluentAssertions;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.Endpoints.Organizers;
using Xunit;

namespace TicketManager.IntegrationTests.Organizers;

public class OrganizerProfileTests : TestBase
{
    [Fact]
    public async Task Organizer_can_see_its_profile()
    {
        var response = await OrganizerClient.GETAsync<OrganizerViewEndpoint, OrganizerViewRequest, OrganizerDto>(new());

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Result.Should().BeEquivalentTo(DefaultOrganizer);
    }
}
