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
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Accounts).ReturnsDbSet(new List<Account>());
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new RegisterUserValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        
        var result = await validator.ValidateAsync(validRequest);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsFree_ItShouldReturnTrue()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Accounts).ReturnsDbSet(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash") });
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new RegisterUserValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        
        var result = await validator.ValidateAsync(validRequest);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsTaken_ItShouldReturnFalseWithEmailIsAlreadyTakenErrorCode()
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Accounts).ReturnsDbSet(new List<Account>() { new(Guid.NewGuid(), "email@email.com", "passwordHash") });
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new RegisterUserValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        
        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterUserRequest.ErrorCodes.EmailIsAlreadyTaken);
    }
}
