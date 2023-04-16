using FastEndpoints;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.ApiKeyAuth;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess.Repositories;

namespace TicketManager.PaymentService.Services.Endpoints.Payments;

public class CreatePaymentEndpoint : EndpointWithoutRequest<PaymentTokenResponse>
{
    private readonly Repository<Payment, Guid> payments;
    private readonly PaymentServiceConfiguration configuration;

    public CreatePaymentEndpoint(Repository<Payment, Guid> payments, PaymentServiceConfiguration configuration)
    {
        this.payments = payments;
        this.configuration = configuration;
    }

    public override void Configure()
    {
        Post("/payment/create");
        PreProcessors(new ApiKeyAuthorizer<EmptyRequest>(configuration));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var payment = new Payment();

        await payments.AddAsync(payment, ct);

        await SendOkAsync(new PaymentTokenResponse() { Id = payment.Id }, ct);
    }
}
