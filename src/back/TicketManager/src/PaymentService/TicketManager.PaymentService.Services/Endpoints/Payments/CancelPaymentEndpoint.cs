using FastEndpoints;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CancelPaymentEndpoint: Endpoint<CancelPaymentRequest, EmptyResponse>
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
        try
        {
            var payment = await payments.FindAndEnsureExistenceAsync(req.Id, ct);

            var result = payment.CancelPayment();
            if (!result)
            {
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await payments.UpdateAsync(payment, ct);
        }
        catch(EntityDoesNotExistException e)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        await SendOkAsync(ct);
    }
}
