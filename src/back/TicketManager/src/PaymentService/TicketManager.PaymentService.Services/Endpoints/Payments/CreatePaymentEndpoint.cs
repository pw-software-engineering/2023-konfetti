using FastEndpoints;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CreatePaymentEndpoint : EndpointWithoutRequest<PaymentTokenResponse>
{
    private readonly Repository<Payment, Guid> _payments;

    public CreatePaymentEndpoint(Repository<Payment, Guid> payments)
    {
        _payments = payments;
    }

    public override void Configure()
    {
        Post("/payment/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var payment = new Payment();

        await _payments.AddAsync(payment, ct);

        await SendAsync(new PaymentTokenResponse() { Token = payment.Id }, cancellation: ct);
    }
}
