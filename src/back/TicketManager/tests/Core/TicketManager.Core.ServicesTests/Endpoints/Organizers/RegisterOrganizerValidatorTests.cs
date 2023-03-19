using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.Mockables;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Organizers;

public class RegisterOrganizerValidatorTests
{
    private readonly RegisterOrganizerRequest validRequest;
    private readonly RegisterOrganizerValidator singleEntryValidator;

    public RegisterOrganizerValidatorTests()
    {
        validRequest = new()
        {
            CompanyName = "company sp. z o. o.",
            Address = "ul. Koszykowa 75, 00-662 Warszawa, Poland",
            TaxId = "3423272256",
            TaxIdType = TaxIdEnum.Nip,
            DisplayName = "Super company",
            Email = "email@emai.com",
            Password = "Password1",
            PhoneNumber = "123-456-789"
        };
        singleEntryValidator = GetValidator(new List<Account>() { new(Guid.NewGuid(), "email2@email.com", "passwordHash", AccountRoles.Organizer) });
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvided_ItShouldReturnTrue()
    {
        var validator = GetValidator(new List<Account>());

        var result = await validator.ValidateAsync(validRequest);

        result.EnsureCorrectness();
    }
    
    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsFree_ItShouldReturnTrue()
    {
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectness();
    }

    [Fact]
    public async Task WhenValidRequestIsProvidedAndEmailIsTaken_ItShouldReturnFalseWithEmailIsAlreadyTakenErrorCode()
    {
        validRequest.Email = "email2@email.com";
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.EmailIsAlreadyTaken);
    }
    
    [Fact]
    public async Task WhenEmptyEmailIsProvided_ItShouldReturnFalseWithEmailIsEmptyErrorCode()
    {
        validRequest.Email = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.EmailIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongEmailIsProvided_ItShouldReturnFalseWithEmailIsTooLongErrorCode()
    {
        validRequest.Email = new string('a', StringLengths.MediumString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.EmailIsTooLong);
    }
    
    [Fact]
    public async Task WhenInvalidEmailIsProvided_ItShouldReturnFalseWithEmailIsInvalidErrorCode()
    {
        validRequest.Email = "invalid email";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.EmailIsInvalid);
    }
    
    [Fact]
    public async Task WhenEmptyPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooShortErrorCode()
    {
        validRequest.Password = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PasswordIsTooShort);
    }

    [Fact] public async Task WhenNullPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooShortErrorCode()
    {
#pragma warning disable CS8625 // nullable
        validRequest.Password = null;
#pragma warning restore CS8625
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PasswordIsNull);
    }
    
    [Fact]
    public async Task WhenTooLongPasswordIsProvided_ItShouldReturnFalseWithPasswordIsTooLongErrorCode()
    {
        validRequest.Password = new string('a', StringLengths.ShortString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PasswordIsTooLong);
    }
    
    [Fact]
    public async Task WhenInvalidPasswordIsProvided_ItShouldReturnFalseWithPasswordIsInvalidErrorCode()
    {
        validRequest.Password = "invalid password";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PasswordIsInvalid);
    }
    
    [Fact]
    public async Task WhenEmptyCompanyNameIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        validRequest.CompanyName = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.CompanyNameIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongCompanyNameIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        validRequest.CompanyName = new string('a', StringLengths.ShortString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.CompanyNameIsTooLong);
    }
    
    [Fact]
    public async Task WhenEmptyAddressIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        validRequest.Address = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.AddressIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongAddressIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        validRequest.Address = new string('a', StringLengths.MediumString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.AddressIsTooLong);
    }
    
    [Fact]
    public async Task WhenEmptyTaxIdIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        validRequest.TaxId = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.TaxIdIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongTaxIdIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        validRequest.TaxId = new string('a', StringLengths.MediumString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.TaxIdIsTooLong);
    }
    
    [Fact]
    public async Task WhenEmptyDisplayNameIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        validRequest.DisplayName = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.DisplayNameIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongDisplayNameIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        validRequest.DisplayName = new string('a', StringLengths.MediumString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.DisplayNameIsTooLong);
    }
    
    [Fact]
    public async Task WhenEmptyPhoneNumberIsProvided_ItShouldReturnFalseWithFirstNameIsEmptyErrorCode()
    {
        validRequest.PhoneNumber = "";
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PhoneNumberIsEmpty);
    }
    
    [Fact]
    public async Task WhenTooLongPhoneNumberIsProvided_ItShouldReturnFalseWithFirstNameIsTooLongErrorCode()
    {
        validRequest.PhoneNumber = new string('a', StringLengths.MediumString + 1);
        
        var result = await singleEntryValidator.ValidateAsync(validRequest);

        result.EnsureCorrectError(RegisterOrganizerRequest.ErrorCodes.PhoneNumberIsTooLong);
    }
    
    
    private static RegisterOrganizerValidator GetValidator(List<Account> accounts)
    {
        var dbContextMock = new Mock<CoreDbContext>(new DbContextOptionsBuilder<CoreDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockableCoreDbResolver>();
        dbContextMock.Setup(d => d.Accounts).ReturnsDbSet(accounts);
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new RegisterOrganizerValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        return validator;
    }
}