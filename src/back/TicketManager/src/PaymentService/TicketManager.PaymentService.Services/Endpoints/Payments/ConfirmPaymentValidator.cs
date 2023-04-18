using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Extensions;
using TicketManager.PaymentService.Services.Services.Mockables;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class ConfirmPaymentValidator: Validator<ConfirmPaymentRequest>
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly MockablePaymentDbResolver dbResolver;
    
    public ConfirmPaymentValidator(IServiceScopeFactory scopeFactory, MockablePaymentDbResolver dbResolver)
    {
        this.scopeFactory = scopeFactory;
        this.dbResolver = dbResolver;

        RuleFor(req => req.Id)
            .MustAsync(IsIdPresentAsync)
            .WithCode(ConfirmPaymentRequest.ErrorCodes.PaymentDoesNotExist)
            .WithMessage("Payment with this Id does not exist")
            .MustAsync(IsNotDecidedAsync)
            .WithCode(CancelPaymentRequest.ErrorCodes.PaymentAlreadyDecided)
            .WithMessage("Payment has been already decided")
            .MustAsync(HasNotExpiredAsync)
            .WithCode(CancelPaymentRequest.ErrorCodes.PaymentHasExpired)
            .WithMessage("Payment has already expired");
    }
    
    private async Task<bool> IsIdPresentAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        return await dbResolver.Resolve(scope)
            .Payments
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
    
    private async Task<bool> IsNotDecidedAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        var payment = await dbResolver.Resolve(scope)
            .Payments
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return !payment?.IsDecided ?? true; 
    }
    
    private async Task<bool> HasNotExpiredAsync(Guid id, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        var payment = await dbResolver.Resolve(scope)
            .Payments
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return !payment?.HasExpired ?? true; 
    }
}
