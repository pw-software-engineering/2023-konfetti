using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.Mockables;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Users;

public class RegisterUserValidatorTests
{
    private readonly RegisterUserRequest validRequest;

    public RegisterUserValidatorTests()
    {
        validRequest = new()
        {
            FirstName = "name",
            LastName = "lastName",
            BirthDate = new DateOnly(2001, 01, 01),
            Email = "email@email.com",
            Password = "password",
        };
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvided_ItShouldReturnTrue()
    {
        var validator = GetValidator(new List<Account>());

        var result = await validator.ValidateAsync(validRequest);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsFree_ItShouldReturnTrue()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });

        var result = await validator.ValidateAsync(validRequest);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsTaken_ItShouldReturnFalseWithEmailIsAlreadyTakenErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email@email.com", "passwordHash") });

        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.EmailIsAlreadyTaken);
    }
    
    [Fact]
    public async Task WhenEmptyEmailIsProvided_ItShouldReturnFalseWithEmailIsEmptyErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Email = "";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.EmailIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongEmailIsProvided_ItShouldReturnFalseWithEmailIsTooLongErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Email = new string('a', StringLengths.MediumString + 1);
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.EmailIsTooLong);
    }
    
    [Fact]
    public async Task WhenInvalidEmailIsProvided_ItShouldReturnFalseWithEmailIsInvalidErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Email = "invalid email";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.EmailIsInvalid);
    }
    
    [Fact]
    public async Task WhenEmptyPasswordIsProvided_ItShouldReturnFalseWithPasswordIsEmptyErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Password = "";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.PasswordIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooLongErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Password = new string('a', StringLengths.ShortString + 1);
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.PasswordIsTooLong);
    }
    
    [Fact(Skip = "Valid password is not specified yet")]
    public async Task WhenInvalidPasswordIsProvided_ItShouldReturnFalseWithPasswordIsInvalidErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.Password = "";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.PasswordIsInvalid);
    }
    
    [Fact]
    public async Task WhenEmptyFirstNameIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.FirstName = "";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.FirstNameIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongFirstNameIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.FirstName = new string('a', StringLengths.ShortString + 1);
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.FirstNameIsTooLong);
    }
    
    [Fact]
    public async Task WhenEmptyLastNameIsProvided_ItShouldReturnFalseWithLastNameIsEmptyErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.LastName = "";
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.LastNameIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongLastNameIsProvided_ItShouldReturnFalseWithLastNameIsTooLongErrorCode()
    {
        var validator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        validRequest.LastName = new string('a', StringLengths.ShortString + 1);
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.LastNameIsTooLong);
    }
    
    private static RegisterUserValidator GetValidator(List<Account> accounts)
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Accounts).ReturnsDbSet(accounts);
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new RegisterUserValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        return validator;
    }
}
