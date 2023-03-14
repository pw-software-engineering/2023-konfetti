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

        [Fact]
        public void WhenConstructorCalledTwice_ItShouldReturnTwoDifferentIds()
        {
            var user1 = new User("email", "name", "lastname", DateOnly.MaxValue);
            var user2 = new User("email", "name", "lastname", DateOnly.MaxValue);

            user1.Id.Should().NotBe(user2.Id);
        }
    }

    public class GetAccountTests
    {
        [Fact]
        public void WhenPasswordHashIsProvided_ItShouldGenerateAccountWithTheSameData()
        {
            var user = new User("email", "firstname", "lastname", new DateOnly(2001, 4, 18));
            var passwordHash = "passwordhash";

            var account = user.GetAccount(passwordHash);

            account.Id.Should().Be(user.Id);
            account.Email.Should().Be(user.Email);
            account.PasswordHash.Should().Be(passwordHash);
        }
    }
}
