using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Users;

public class UpdateUserValidator : Validator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        When(req => req.Email is not null, () =>
        {
            RuleFor(req => req.Email)
                .NotEmpty()
                .WithCode(UpdateUserRequest.ErrorCodes.EmailIsEmpty)
                .MaximumLength(StringLengths.MediumString)
                .WithCode(UpdateUserRequest.ErrorCodes.EmailIsTooLong)
                .EmailAddress()
                .WithCode(UpdateUserRequest.ErrorCodes.EmailIsInvalid);

            RuleFor(req => req)
                .MustAsync(IsEmailAvailable)
                .WithCode(UpdateUserRequest.ErrorCodes.EmailIsAlreadyTaken)
                .WithMessage("Email is already taken");
        });

        When(req => req.FirstName is not null, () =>
        {
            RuleFor(req => req.FirstName)
                .NotEmpty()
                .WithCode(UpdateUserRequest.ErrorCodes.FirstNameIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateUserRequest.ErrorCodes.FirstNameIsTooLong);
        });

        When(req => req.LastName is not null, () =>
        {
            RuleFor(req => req.LastName)
                .NotEmpty()
                .WithCode(UpdateUserRequest.ErrorCodes.LastNameIsEmpty)
                .MaximumLength(StringLengths.ShortString)
                .WithCode(UpdateUserRequest.ErrorCodes.LastNameIsTooLong);
        });
    }
    
    private async Task<bool> IsEmailAvailable(UpdateUserRequest req, CancellationToken cancellationToken)
    {
        return await Resolve<CoreDbContext>()
            .Accounts
            .AllAsync(a => a.Email != req.Email || a.Id == req.AccountId, cancellationToken);
    }
}
