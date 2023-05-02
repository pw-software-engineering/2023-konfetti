using FastEndpoints;
using FluentValidation;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountUpdatePasswordValidator: Validator<AccountUpdatePasswordRequest>
{
    public AccountUpdatePasswordValidator()
    {
        RuleFor(req => req.Password)
            .NotNull()
            .WithCode(AccountUpdatePasswordRequest.ErrorCodes.PasswordIsNull)
            .SetValidator(new PasswordValidator(
                AccountUpdatePasswordRequest.ErrorCodes.PasswordIsTooShort,
                AccountUpdatePasswordRequest.ErrorCodes.PasswordIsTooLong,
                AccountUpdatePasswordRequest.ErrorCodes.PasswordIsInvalid));
    }
}
