using FastEndpoints;
using FluentValidation;
using TicketManager.Core.Contracts.Events;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.ValidationExtensions;

namespace TicketManager.Core.Services.Endpoints.Events;

public class CreateEventValidator : Validator<CreateEventRequest>
{
    public CreateEventValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithCode(CreateEventRequest.ErrorCodes.NameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(CreateEventRequest.ErrorCodes.NameIsTooLong);

        RuleFor(req => req.Description)
            .NotEmpty()
            .WithCode(CreateEventRequest.ErrorCodes.DescriptionIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(CreateEventRequest.ErrorCodes.DescriptionIsTooLong);

        RuleFor(req => req.Location)
            .NotEmpty()
            .WithCode(CreateEventRequest.ErrorCodes.LocationIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(CreateEventRequest.ErrorCodes.LocationIsTooLong);

        RuleFor(req => req.Date)
            .Must(IsDateFuture)
            .WithCode(CreateEventRequest.ErrorCodes.DateIsNotFuture)
            .WithMessage("Date is not future");

        RuleFor(req => req.Sectors)
            .NotEmpty()
            .WithCode(CreateEventRequest.ErrorCodes.SectorsAreEmpty)
            .Must(AreSectorsNamesUnique)
            .WithCode(CreateEventRequest.ErrorCodes.SectorNamesAreNotUnique);

        RuleForEach(req => req.Sectors)
            .SetValidator(new SectorValidator());
    }

    private bool IsDateFuture(DateTime date)
    {
        return date > DateTime.UtcNow;
    }

    private bool AreSectorsNamesUnique(List<SectorDto> sectors)
    {
        return sectors.DistinctBy(s => s.Name).Count() == sectors.Count;
    }
}
