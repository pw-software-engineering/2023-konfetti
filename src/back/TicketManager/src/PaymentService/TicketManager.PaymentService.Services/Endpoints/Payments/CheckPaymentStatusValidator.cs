using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Extensions;
using TicketManager.PaymentService.Services.Services.Mockables;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CheckPaymentStatusValidator: Validator<CheckPaymentStatusRequest>
{
    public CheckPaymentStatusValidator()
    {
        RuleFor(req => req.Id)
            .MustAsync(IsIdPresentAsync)
            .WithCode(CheckPaymentStatusRequest.ErrorCodes.PaymentDoesNotExist)
            .WithMessage("Payment with this Id does not exist");
    }
    
    private async Task<bool> IsIdPresentAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Resolve<PaymentDbContext>()
            .Payments
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
}
