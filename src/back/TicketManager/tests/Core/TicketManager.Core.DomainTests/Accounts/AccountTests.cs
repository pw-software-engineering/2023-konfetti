using FluentAssertions;
using TicketManager.Core.Domain.Accounts;
using Xunit;

namespace TicketManager.Core.DomainTests.Accounts;

public class AccountTests
{
    public class ConstructorTests
    {
        [Theory]
        [InlineData("617bf402-dd16-4e0d-8fd2-bc66429eee35", "email@email.com", "passwordHash", AccountRoles.User)]
        [InlineData("b41d4f5b-5465-44dd-ab4f-e8f215c9913b", "michal@gmail.com", "passwordHash2", AccountRoles.Organizer)]
        [InlineData("e840230d-00ca-430a-9eb4-ab935346d34b", "kuba@email.com", "passwordHash3", AccountRoles.Admin)]
        [InlineData("98632145-9618-44d5-9623-71a3023bdd54", "lionel@messi.com", "passwordHash4", "some role")]
        public void WhenValidParametersProvided_ItShouldSetThemAsProperties(string idAsString, string email, string passwordHash, string role)
        {
            var id = new Guid(idAsString);
            var account = new Account(id, email, passwordHash, role);

            account.Id.Should().Be(id);
            account.Email.Should().Be(email);
            account.PasswordHash.Should().Be(passwordHash);
            account.Role.Should().Be(role);
        }
    }
}
