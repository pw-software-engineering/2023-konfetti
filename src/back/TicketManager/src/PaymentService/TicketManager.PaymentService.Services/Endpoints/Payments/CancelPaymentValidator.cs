using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Extensions;
using TicketManager.PaymentService.Services.Services.Mockables;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CancelPaymentValidator : Validator<CancelPaymentRequest>
{
    public CancelPaymentValidator()
    {
        RuleFor(req => req.Id)
            .MustAsync(IsIdPresentAsync)
            .WithCode(CancelPaymentRequest.ErrorCodes.PaymentDoesNotExist)
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
        return await Resolve<PaymentDbContext>()
            .Payments
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
    
    private async Task<bool> IsNotDecidedAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await Resolve<PaymentDbContext>()
            .Payments
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return !payment?.IsDecided ?? true; 
    }
    
    private async Task<bool> HasNotExpiredAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await Resolve<PaymentDbContext>()
            .Payments
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return !payment?.HasExpired ?? true; 
    }
}
