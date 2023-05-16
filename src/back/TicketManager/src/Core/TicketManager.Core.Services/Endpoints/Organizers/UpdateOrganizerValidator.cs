using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class UpdateOrganizerValidator : Validator<UpdateOrganizerRequest>
{
    public UpdateOrganizerValidator()
    {
        When(req => req.Email != null, () =>
        {
            RuleFor(req => req.Email)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.EmailIsEmpty)
                .MaximumLength(StringLengths.MediumString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.EmailIsTooLong)
                .EmailAddress()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.EmailIsInvalid);

            RuleFor(req => req)
                .MustAsync(IsEmailAvailableAsync)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.EmailIsAlreadyTakenByAnotherAccount)
                .WithMessage("Email is already taken by another account");
        });

        When(req => req.CompanyName != null, () =>
        {
            RuleFor(req => req.CompanyName)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.CompanyNameIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.CompanyNameIsTooLong);
        });

        When(req => req.Address != null, () =>
        {
            RuleFor(req => req.Address)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.AddressIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.AddressIsTooLong);
        });

        When(req => req.TaxId != null, () =>
        {
            RuleFor(req => req.TaxId)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.TaxIdIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.TaxIdIsTooLong);
        });

        RuleFor(req => req.TaxIdType)
            .IsInEnum()
            .WithCode(UpdateOrganizerRequest.ErrorCodes.TaxIdTypeIsInvalid);

        When(req => req.DisplayName != null, () =>
        {
            RuleFor(req => req.DisplayName)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.DisplayNameIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.DisplayNameIsTooLong);
        });

        When(req => req.PhoneNumber != null, () =>
        {
            RuleFor(req => req.PhoneNumber)
                .NotEmpty()
                .WithCode(UpdateOrganizerRequest.ErrorCodes.PhoneNumberIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateOrganizerRequest.ErrorCodes.PhoneNumberIsTooLong);
        });
    }
    
    private async Task<bool> IsEmailAvailableAsync(UpdateOrganizerRequest req, CancellationToken cancellationToken)
    {
        return await Resolve<CoreDbContext>()
            .Accounts
            .AllAsync(a => a.Email != req.Email || a.Id == req.AccountId, cancellationToken);
    }
}
