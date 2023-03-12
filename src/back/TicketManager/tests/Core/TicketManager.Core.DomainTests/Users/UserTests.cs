using FluentAssertions;
using TicketManager.Core.Domain.Users;
using Xunit;

namespace TicketManager.Core.DomainTests.Users;

public class UserTests
{
    public class ConstructorTests
    {
        [Theory]
        [InlineData("email@email.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("michal@gmail.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("kuba@email.com", "name", "lastName", 2019, 1, 20)]
        [InlineData("lionel@messi.com", "name", "lastName", 2019, 1, 20)]
        public void WhenValidParametersProvided_ItShouldSetThemAsProperties(
            string email,
            string firstName,
            string lastName,
            int year,
            int month,
            int day)
        {
            var birthDate = new DateOnly(year, month, day);
            var user = new User(email, firstName, lastName, birthDate);

            user.Email.Should().Be(email);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.BirthDate.Should().Be(birthDate);
        }
    }
}
