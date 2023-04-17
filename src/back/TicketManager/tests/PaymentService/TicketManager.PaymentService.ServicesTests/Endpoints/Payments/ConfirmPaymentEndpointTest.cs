using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.Repositories;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class ConfirmPaymentEndpointTest
{
    private readonly ConfirmPaymentEndpoint endpoint;
    private readonly Mock<Repository<Payment, Guid>> paymentsMock;
    
    public ConfirmPaymentEndpointTest()
    {
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        var dbContext = dbContextMock.Object;
        paymentsMock = new Mock<Repository<Payment, Guid>>(dbContext);
        var payments = paymentsMock.Object;
        endpoint = Factory.Create<ConfirmPaymentEndpoint>(payments);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndIsSuccessful_ItShouldConfirmPayment()
    {
        var payment = new Payment();
        paymentsMock.Setup(p => p.FindAndEnsureExistenceAsync(payment.Id, default)).ReturnsAsync(payment);
        
        await endpoint.HandleAsync(new ConfirmPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Once);
        
        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
