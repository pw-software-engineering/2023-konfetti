using FluentValidation;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class SectorValidator : AbstractValidator<SectorDto>
{
    public SectorValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithCode(SectorDto.ErrorCodes.NameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(SectorDto.ErrorCodes.NameIsTooLong);

        RuleFor(req => req.PriceInSmallestUnit)
            .GreaterThan(0)
            .WithCode(SectorDto.ErrorCodes.PriceInSmallestUnitIsNotPositive);

        RuleFor(req => req.NumberOfColumns)
            .GreaterThan(0)
            .WithCode(SectorDto.ErrorCodes.NumberOfColumnsIsNotPositive);

        RuleFor(req => req.NumberOfRows)
            .GreaterThan(0)
            .WithCode(SectorDto.ErrorCodes.NumberOfRowsIsNotPositive);
    }
}
