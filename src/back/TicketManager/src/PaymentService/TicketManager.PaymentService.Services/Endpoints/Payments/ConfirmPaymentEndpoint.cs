using FastEndpoints;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class ConfirmPaymentEndpoint: Endpoint<ConfirmPaymentRequest>
{
    private readonly Repository<Payment, Guid> payments;

    public ConfirmPaymentEndpoint(Repository<Payment,Guid> payments)
    {
        this.payments = payments;
    }

    public override void Configure()
    {
        Post("/payment/confirm");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ConfirmPaymentRequest req, CancellationToken ct)
    {
        var payment = await payments.FindAndEnsureExistenceAsync(req.Id, ct);
        
        payment.ConfirmPayment();
        
        await payments.UpdateAsync(payment, ct);
       
        await SendOkAsync(ct);
    }
}
