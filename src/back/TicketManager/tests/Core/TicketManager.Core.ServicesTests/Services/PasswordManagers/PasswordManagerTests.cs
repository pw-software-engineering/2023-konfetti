using FluentAssertions;
using TicketManager.Core.Services.Services.PasswordManagers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Services.PasswordManagers;

public class PasswordManagerTests
{
    public class GetHashTests
    {
        private readonly PasswordManager passwordManager = new();
        
        [Theory]
        [InlineData("password")]
        [InlineData("password1")]
        [InlineData("haslo")]
        [InlineData("asdf qwer 123")]
        public void WhenPasswordProvided_ItShouldReturnHashDifferentThanPassword(string password)
        {
            var hash = passwordManager.GetHash(password);

            hash.Should().NotBe(password);
        } 
    }
}
