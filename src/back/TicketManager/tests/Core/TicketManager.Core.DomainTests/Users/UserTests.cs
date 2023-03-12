using FluentAssertions;
using TicketManager.Core.Domain.Users;
using Xunit;

namespace TicketManager.Core.DomainTests.Users;

public class UserTests
{
    public class ConstructorTests
    {
        [Theory]
        [InlineData("617bf402-dd16-4e0d-8fd2-bc66429eee35", "email@email.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("b41d4f5b-5465-44dd-ab4f-e8f215c9913b", "michal@gmail.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("e840230d-00ca-430a-9eb4-ab935346d34b", "kuba@email.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("98632145-9618-44d5-9623-71a3023bdd54", "lionel@messi.com", "name", "lastName", 2019, 1, 20)]
        public void WhenValidParametersProvided_ItShouldSetThemAsProperties(
            string idAsString,
            string email,
            string firstName,
            string lastName,
            int year,
            int month,
            int day)
        {
            var id = new Guid(idAsString);
            var birthDate = new DateOnly(year, month, day);
            var user = new User(id, email, firstName, lastName, birthDate);

            user.Id.Should().Be(id);
            user.Email.Should().Be(email);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.BirthDate.Should().Be(birthDate);
        }
    }
}
