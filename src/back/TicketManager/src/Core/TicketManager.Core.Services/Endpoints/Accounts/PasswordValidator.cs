using FluentValidation;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class PasswordValidator : AbstractValidator<string>
{
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 32;

    public PasswordValidator(
        int passwordIsTooShortErrorCode = 1,
        int passwordIsTooLongErrorCode = 2,
        int passwordIsInvalidErrorCode = 3)
    {
        RuleFor(password => password)
            .MinimumLength(PasswordMinLength)
            .WithCode(passwordIsTooShortErrorCode)
            .MaximumLength(PasswordMaxLength)
            .WithCode(passwordIsTooLongErrorCode)
            .Must(IsPasswordValid)
            .WithCode(passwordIsInvalidErrorCode)
            .WithMessage("Password is invalid");
    }

    private bool IsPasswordValid(string password)
    {
        return AreAllCharactersValid(password) &&
               DoesContainSmallLetter(password) &&
               DoesContainBigLetter(password) &&
               DoesContainDigit(password);
    }

    private bool AreAllCharactersValid(string password)
    {
        return password.All(c => c >= 33 && c <= 126);
    }

    private bool DoesContainSmallLetter(string password)
    {
        return password.Any(c => c is >= 'a' and <= 'z');
    }
    
    private bool DoesContainBigLetter(string password)
    {
        return password.Any(c => c is >= 'A' and <= 'Z');
    }
    
    private bool DoesContainDigit(string password)
    {
        return password.Any(c => c is >= '0' and <= '9');
    }
}
