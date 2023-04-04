using FluentAssertions;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Users;

public class UserProfileTests : TestBase
{
    [Fact]
    public async Task User_can_see_its_profile()
    {
        var profile = await UserClient.GetSuccessAsync<UserViewEndpoint, UserViewRequest, UserDto>(new());
        
        profile.Should().BeEquivalentTo(DefaultUser);
    }
}
