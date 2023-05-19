using FastEndpoints;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CancelPaymentEndpoint : Endpoint<CancelPaymentRequest>
{
    private readonly Repository<Payment, Guid> payments;

    public CancelPaymentEndpoint(Repository<Payment,Guid> payments)
    {
        this.payments = payments;
    }

    public override void Configure()
    {
        Post("/payment/cancel");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancelPaymentRequest req, CancellationToken ct)
    {
        var payment = await payments.FindAndEnsureExistenceAsync(req.Id, ct);

        payment.CancelPayment();

        await payments.UpdateAsync(payment, ct);
        
        await SendOkAsync(ct);
    }
}
