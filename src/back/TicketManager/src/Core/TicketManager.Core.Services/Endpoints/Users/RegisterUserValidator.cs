using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Extensions;
using TicketManager.Core.Services.Services.Mockables;

namespace TicketManager.Core.Services.Endpoints.Users;

public class RegisterUserValidator : Validator<RegisterUserRequest>
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly MockableCoreDbResolver dbResolver;
    
    public RegisterUserValidator(IServiceScopeFactory scopeFactory, MockableCoreDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;
        
        RuleFor(req => req.Email)
            .NotEmpty()
            .WithCode(RegisterUserRequest.ErrorCodes.EmailIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(RegisterUserRequest.ErrorCodes.EmailIsTooLong)
            .MustAsync(IsEmailAvailable)
            .WithCode(RegisterUserRequest.ErrorCodes.EmailIsAlreadyTaken)
            .WithMessage("Email is already taken")
            .EmailAddress()
            .WithCode(RegisterUserRequest.ErrorCodes.EmailIsInvalid);

        RuleFor(req => req.Password)
            .NotNull()
            .WithCode(RegisterUserRequest.ErrorCodes.PasswordIsNull)
            .SetValidator(new PasswordValidator(
                RegisterUserRequest.ErrorCodes.PasswordIsTooShort,
                RegisterUserRequest.ErrorCodes.PasswordIsTooLong,
                RegisterUserRequest.ErrorCodes.PasswordIsInvalid));

        RuleFor(req => req.FirstName)
            .NotEmpty()
            .WithCode(RegisterUserRequest.ErrorCodes.FirstNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterUserRequest.ErrorCodes.FirstNameIsTooLong);

        RuleFor(req => req.LastName)
            .NotEmpty()
            .WithCode(RegisterUserRequest.ErrorCodes.LastNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterUserRequest.ErrorCodes.LastNameIsTooLong);
    }

    private async Task<bool> IsEmailAvailable(string email, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Accounts
            .AllAsync(a => a.Email != email, cancellationToken);
    }
}
