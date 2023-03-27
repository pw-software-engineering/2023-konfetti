using System.Net;
using FastEndpoints;
using FluentAssertions;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Services.Endpoints.Users;
using Xunit;

namespace TicketManager.IntegrationTests.Users;

public class UserProfileTests : TestBase
{
    [Fact]
    public async Task User_can_see_its_profile()
    {
        var response = await UserClient.GETAsync<UserViewEndpoint, UserViewRequest, UserDto>(new());

        response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Result.Should().BeEquivalentTo(DefaultUser);
    }
}
