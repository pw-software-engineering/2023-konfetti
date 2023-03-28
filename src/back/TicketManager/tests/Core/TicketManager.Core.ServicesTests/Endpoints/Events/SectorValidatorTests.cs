using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Events;
using TicketManager.Core.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.Core.ServicesTests.Endpoints.Events;

public class SectorValidatorTests
{
    private readonly SectorDto sector;
    private readonly SectorValidator validator = new();

    public SectorValidatorTests()
    {
        sector = new()
        {
            Name = "name",
            PriceInSmallestUnit = 4,
            NumberOfColumns = 5,
            NumberOfRows = 10,
        };
    }

    [Fact]
    public void WhenValidSectorIsProvided_ItShouldReturnTrue()
    {
        var result = validator.Validate(sector);
        
        result.EnsureCorrectness();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\n")]
    [InlineData("\t")]
    public void WhenEmptyNameIsProvided_ItShouldReturnFalseWithNameIsEmptyErrorCode(string name)
    {
        sector.Name = name;
        var result = validator.Validate(sector);
        
        result.EnsureCorrectError(SectorDto.ErrorCodes.NameIsEmpty);
    }
    
    [Fact]
    public void WhenTooLongNameIsProvided_ItShouldReturnFalseWithNameIsTooLongErrorCode()
    {
        sector.Name = new string('a', StringLengths.ShortString + 1);
        var result = validator.Validate(sector);
        
        result.EnsureCorrectError(SectorDto.ErrorCodes.NameIsTooLong);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1213)]
    public void WhenNotPositivePriceIsProvided_ItShouldReturnFalseWithPriceInSmallestUnitIsNotPositiveErrorCode(int price)
    {
        sector.PriceInSmallestUnit = price;
        var result = validator.Validate(sector);
        
        result.EnsureCorrectError(SectorDto.ErrorCodes.PriceInSmallestUnitIsNotPositive);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1213)]
    public void WhenNotPositiveNumberOfColumnsIsProvided_ItShouldReturnFalseWithNumberOfColumnsIsNotPositiveErrorCode(int numberOfColumns)
    {
        sector.NumberOfColumns = numberOfColumns;
        var result = validator.Validate(sector);
        
        result.EnsureCorrectError(SectorDto.ErrorCodes.NumberOfColumnsIsNotPositive);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1213)]
    public void WhenNotPositiveNumberOfRowsIsProvided_ItShouldReturnFalseWithNumberOfRowsIsNotPositiveErrorCode(int numberOfRows)
    {
        sector.NumberOfRows = numberOfRows;
        var result = validator.Validate(sector);
        
        result.EnsureCorrectError(SectorDto.ErrorCodes.NumberOfRowsIsNotPositive);
    }
}
