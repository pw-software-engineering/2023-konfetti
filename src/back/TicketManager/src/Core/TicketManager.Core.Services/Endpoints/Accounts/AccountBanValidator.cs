using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Extensions;

namespace TicketManager.Core.Services.Endpoints.Accounts;

public class AccountBanValidator : Validator<AccountBanRequest>
{
    public AccountBanValidator()
    {
        RuleFor(req => req.AccountId)
            .MustAsync(DoesAccountExistAsync)
            .WithCode(AccountBanRequest.ErrorCodes.AccountDoesNotExist)
            .WithMessage("Account does not exist");
    }

    private Task<bool> DoesAccountExistAsync(Guid id, CancellationToken ct)
    {
        return Resolve<CoreDbContext>()
            .Accounts
            .AnyAsync(a => a.Id == id, ct);
    }
}
