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
    private readonly Mock<Payment> paymentMock;
    private readonly Payment payment;
    private readonly Mock<Repository<Payment, Guid>> paymentsMock;
    
    public ConfirmPaymentEndpointTest()
    {
        paymentMock = new Mock<Payment>();
        payment = paymentMock.Object;
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        var dbContext = dbContextMock.Object;
        paymentsMock = new Mock<Repository<Payment, Guid>>(dbContext);
        paymentsMock.Setup(p => p.FindAndEnsureExistenceAsync(payment.Id, default)).ReturnsAsync(payment);
        var payments = paymentsMock.Object;
        endpoint = Factory.Create<ConfirmPaymentEndpoint>(payments);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndIsSuccessful_ItShouldConfirmPayment()
    {
        await endpoint.HandleAsync(new ConfirmPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.ConfirmPayment(), Times.Once);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Once);
        
        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndIsUnsuccessful_ItShouldNotConfirmPayment()
    {
        paymentMock.Setup(p => p.ConfirmPayment()).Throws(new PaymentAlreadyDecidedOrExpiredException());

        
        await endpoint.HandleAsync(new ConfirmPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.ConfirmPayment(), Times.Once);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Never);

        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndPaymentIsNonExisting_ItShouldReturn400()
    {
        paymentsMock
            .Setup(p => p.FindAndEnsureExistenceAsync(payment.Id, default))
            .Throws(new EntityDoesNotExistException());
        
        await endpoint.HandleAsync(new ConfirmPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.ConfirmPayment(), Times.Never);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Never);

        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
