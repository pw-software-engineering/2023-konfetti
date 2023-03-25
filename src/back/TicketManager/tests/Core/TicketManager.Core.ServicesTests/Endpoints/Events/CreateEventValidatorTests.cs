using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Events;

public class CreateEventValidatorTests
{
    private readonly CreateEventRequest request;
    private readonly CreateEventValidator validator = new();

    public CreateEventValidatorTests()
    {
        request = new()
        {
            AccountId = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Location = "location",
            Date = DateTime.UtcNow.AddDays(1),
            Sectors = new()
            {
                new()
                {
                    Name = "name1",
                    PriceInSmallestUnit = 1,
                    NumberOfColumns = 1,
                    NumberOfRows = 1,
                },
                new()
                {
                    Name = "name2",
                    PriceInSmallestUnit = 2,
                    NumberOfColumns = 2,
                    NumberOfRows = 2,
                },
                new()
                {
                    Name = "name3",
                    PriceInSmallestUnit = 3,
                    NumberOfColumns = 3,
                    NumberOfRows = 3,
                },
            },
        };
    }

    [Fact]
    public void WhenValidRequestIsProvided_ItShouldReturnTrue()
    {
        var result = validator.Validate(request);
        
        result.EnsureCorrectness();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("  \n\n\t \t \n ")]
    public void WhenEmptyNameIsProvided_ItShouldReturnFalseWithNameIsEmptyErrorCode(string name)
    {
        request.Name = name;
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.NameIsEmpty);
    }

    [Fact]
    public void WhenTooLongNameIsProvided_ItShouldReturnFalseWithNameIsTooLongErrorCode()
    {
        request.Name = new string('a', StringLengths.ShortString + 1);
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.NameIsTooLong);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("  \n\n\t \t \n ")]
    public void WhenEmptyDescriptionIsProvided_ItShouldReturnFalseWithDescriptionIsEmptyErrorCode(string description)
    {
        request.Description = description;
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.DescriptionIsEmpty);
    }

    [Fact]
    public void WhenTooLongDescriptionIsProvided_ItShouldReturnFalseWithDescriptionIsTooLongErrorCode()
    {
        request.Description = new string('a', StringLengths.VeryLongString + 1);
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.DescriptionIsTooLong);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("  \n\n\t \t \n ")]
    public void WhenEmptyLocationIsProvided_ItShouldReturnFalseWithLocationIsEmptyErrorCode(string location)
    {
        request.Location = location;
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.LocationIsEmpty);
    }

    [Fact]
    public void WhenTooLongLocationIsProvided_ItShouldReturnFalseWithLocationIsTooLongErrorCode()
    {
        request.Location = new string('a', StringLengths.MediumString + 1);
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.LocationIsTooLong);
    }

    [Fact]
    public void WhenPastDateIsProvided_ItShouldReturnFalseWithDateIsNotFutureErrorCode()
    {
        request.Date = DateTime.UtcNow.AddDays(-1);
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.DateIsNotFuture);
    }

    [Fact]
    public void WhenEmptySectorsListIsProvided_ItShouldReturnFalseWithSectorsAreEmptyErrorCode()
    {
        request.Sectors = new List<SectorDto>();
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.SectorsAreEmpty);
    }
    
    [Fact]
    public void WhenSameSectorNamesAreProvided_ItShouldReturnFalseWithSectorNamesAreNotUniqueErrorCode()
    {
        request.Sectors = new()
        {
            new()
            {
                Name = "name",
                NumberOfRows = 1,
                NumberOfColumns = 1,
                PriceInSmallestUnit = 1,
            },
            new()
            {
                Name = "name",
                NumberOfRows = 1,
                NumberOfColumns = 1,
                PriceInSmallestUnit = 1,
            },
        };
        var result = validator.Validate(request);
        
        result.EnsureCorrectError(CreateEventRequest.ErrorCodes.SectorNamesAreNotUnique);
    }
}
