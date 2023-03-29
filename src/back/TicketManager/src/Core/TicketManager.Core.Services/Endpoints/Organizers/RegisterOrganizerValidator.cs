using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Extensions;
using TicketManager.Core.Services.Services.Mockables;

namespace TicketManager.Core.Services.Endpoints.Organizers;

public class RegisterOrganizerValidator: Validator<RegisterOrganizerRequest>
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly MockableCoreDbResolver dbResolver;
    
    public RegisterOrganizerValidator(IServiceScopeFactory scopeFactory, MockableCoreDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;
        
        RuleFor(req => req.Email)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.EmailIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.EmailIsTooLong)
            .MustAsync(IsEmailAvailableAsync)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.EmailIsAlreadyTaken)
            .WithMessage("Email is already taken")
            .EmailAddress()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.EmailIsInvalid);
        
        RuleFor(req => req.Password)
            .NotNull()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.PasswordIsNull)
            .SetValidator(new PasswordValidator(
                RegisterOrganizerRequest.ErrorCodes.PasswordIsTooShort,
                RegisterOrganizerRequest.ErrorCodes.PasswordIsTooLong,
                RegisterOrganizerRequest.ErrorCodes.PasswordIsInvalid));
        
        RuleFor(req => req.CompanyName)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.CompanyNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.CompanyNameIsTooLong);
        
        RuleFor(req => req.Address)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.AddressIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.AddressIsTooLong);
        
        RuleFor(req => req.TaxId)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.TaxIdIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.TaxIdIsTooLong);
        
        RuleFor(req => req.DisplayName)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.DisplayNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.DisplayNameIsTooLong);
        
        RuleFor(req => req.PhoneNumber)
            .NotEmpty()
            .WithCode(RegisterOrganizerRequest.ErrorCodes.PhoneNumberIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithCode(RegisterOrganizerRequest.ErrorCodes.PhoneNumberIsTooLong);
        
    }
    
    private async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Accounts
            .AllAsync(a => a.Email != email, cancellationToken);
    }
}
