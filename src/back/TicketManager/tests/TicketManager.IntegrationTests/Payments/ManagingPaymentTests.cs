using FastEndpoints;
using FluentAssertions;
using TicketManager.IntegrationTests.Extensions;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.IntegrationTests.Payments;

public class ManagingPaymentTests : TestBase
{
    private readonly List<PaymentDto> payments = new();

    [Fact]
    public async Task Managing_payments_work_correctly()
    {
        await CreatePaymentAsync();
        await VerifyPaymentsAsync();
        
        await CreatePaymentAsync();
        await VerifyPaymentsAsync();
        
        await CreatePaymentAsync();
        await VerifyPaymentsAsync();

        await ConfirmPaymentAsync(payments[0].Id);
        await VerifyPaymentsAsync();
        
        await CancelPaymentAsync(payments[1].Id);
        await VerifyPaymentsAsync();
        
        await VerifyValidationFailuresAsync();
    }

    private async Task CreatePaymentAsync()
    {
        var payment = await PaymentClient.PostSuccessAsync<CreatePaymentEndpoint, EmptyRequest, PaymentTokenResponse>(new());
        payments.Add(new()
        {
            Id = payment.Id,
            PaymentStatus = PaymentStatusDto.Created,
        });
    }
    
    private async Task ConfirmPaymentAsync(Guid paymentId)
    {
        await PaymentClient.PostSuccessAsync<ConfirmPaymentEndpoint, ConfirmPaymentRequest>(new()
        {
            Id = paymentId,
        });
        payments.First(p => p.Id == paymentId).PaymentStatus = PaymentStatusDto.Confirmed;
    }
    
    private async Task CancelPaymentAsync(Guid paymentId)
    {
        await PaymentClient.PostSuccessAsync<CancelPaymentEndpoint, CancelPaymentRequest>(new()
        {
            Id = paymentId,
        });
        payments.First(p => p.Id == paymentId).PaymentStatus = PaymentStatusDto.Cancelled;
    }

    private async Task VerifyPaymentsAsync()
    {
        var actual = await PaymentClient.GetSuccessAsync<ListPaymentsEndpoint, EmptyRequest, List<PaymentDto>>(new());
        actual.Should().BeEquivalentTo(payments, options => options.Excluding(p => p.DateCreated));
    }
    
    private async Task VerifyValidationFailuresAsync()
    {
        await PaymentClient.PostFailureAsync<ConfirmPaymentEndpoint, ConfirmPaymentRequest>(new()
        {
            Id = payments[0].Id,
        });
        await PaymentClient.PostFailureAsync<CancelPaymentEndpoint, CancelPaymentRequest>(new()
        {
            Id = payments[0].Id,
        });
    }
}
