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

        [Theory]
        [InlineData("password")]
        [InlineData("password1")]
        [InlineData("haslo")]
        [InlineData("asdf qwer 123")]
        public void WhenCalledTwice_ItShouldGenerateDifferentHashes(string password)
        {
            var hash1 = passwordManager.GetHash(password);
            var hash2 = passwordManager.GetHash(password);

            hash1.Should().NotBe(hash2);
        }
    }

    public class DoPasswordsMatchTests
    {
        private readonly PasswordManager passwordManager = new();
        
        [Theory]
        [InlineData("password")]
        [InlineData("password1")]
        [InlineData("haslo")]
        [InlineData("asdf qwer 123")]
        public void WhenCalledWithSamePassword_ItShouldReturnTrue(string password)
        {
            var hash = passwordManager.GetHash(password);

            var result = passwordManager.DoPasswordsMatch(hash, password);

            result.Should().BeTrue();
        }
        
        [Theory]
        [InlineData("password")]
        [InlineData("password1")]
        [InlineData("haslo")]
        [InlineData("asdf qwer 123")]
        public void WhenCalledWithDifferentPassword_ItShouldReturnFalse(string password)
        {
            var differentPassword = "different password";
            var hash = passwordManager.GetHash(password);

            var result = passwordManager.DoPasswordsMatch(hash, differentPassword);

            result.Should().BeFalse();
        }
    }
}
