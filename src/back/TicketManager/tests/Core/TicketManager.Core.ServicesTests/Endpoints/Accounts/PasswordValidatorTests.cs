using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Accounts;

public class PasswordValidatorTests
{
    private const int TooShortErrorCode = 1;
    private const int TooLongErrorCode = 2;
    private const int InvalidErrorCode = 3;
    private readonly PasswordValidator validator = new(TooShortErrorCode, TooLongErrorCode, InvalidErrorCode);
    
    [Theory]
    [InlineData("")]
    [InlineData("asdf")]
    [InlineData("asdfasd")]
    public void WhenTooShortPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooShortErrorCode(string password)
    {
        var result = validator.Validate(password);
        
        result.EnsureCorrectError(TooShortErrorCode);
    }
    
    [Theory]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void WhenTooLongPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooLongErrorCode(string password)
    {
        var result = validator.Validate(password);
        
        result.EnsureCorrectError(TooLongErrorCode);
    }
    
    [Theory]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("AAAAAAAAAAAAAA")]
    [InlineData("123123123123")]
    [InlineData("aaaaaa1111111###")]
    [InlineData("AAAAAA1111111###")]
    public void WhenInvalidPasswordIsProvided_ItShouldReturnFalseWithPasswordIsInvalidErrorCode(string password)
    {
        var result = validator.Validate(password);
        
        result.EnsureCorrectError(InvalidErrorCode);
    }
    
    [Theory]
    [InlineData("aaaaaaaaaaaaAAAAAAAAAA1111111111")]
    [InlineData("aaAA11##")]
    [InlineData("valid123Password")]
    public void WhenValidPasswordIsProvided_ItShouldReturnTrue(string password)
    {
        var result = validator.Validate(password);
        
        result.EnsureCorrectness();
    }
}
