using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.Extensions;
using TicketManager.PaymentService.Services.Services.Mockables;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CheckPaymentStatusValidator: Validator<CheckPaymentStatusRequest>
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly MockablePaymentDbResolver dbResolver;
    
    public CheckPaymentStatusValidator(IServiceScopeFactory scopeFactory, MockablePaymentDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;

        RuleFor(req => req.Id)
            .MustAsync(IsIdPresentAsync)
            .WithCode(CheckPaymentStatusRequest.ErrorCodes.PaymentDoesNotExist)
            .WithMessage("Payment with this Id does not exist");
    }
    
    private async Task<bool> IsIdPresentAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Payments
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
}
