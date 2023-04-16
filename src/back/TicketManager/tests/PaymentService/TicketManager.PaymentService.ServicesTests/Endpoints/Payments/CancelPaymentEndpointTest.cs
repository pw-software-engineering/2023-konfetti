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

public class CancelPaymentEndpointTest
{
    private readonly CancelPaymentEndpoint endpoint;
    private readonly Mock<Payment> paymentMock;
    private readonly Payment payment;
    private readonly Mock<Repository<Payment, Guid>> paymentsMock;
    
    public CancelPaymentEndpointTest()
    {
        paymentMock = new Mock<Payment>();
        payment = paymentMock.Object;
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        var dbContext = dbContextMock.Object;
        paymentsMock = new Mock<Repository<Payment, Guid>>(dbContext);
        paymentsMock.Setup(p => p.FindAndEnsureExistenceAsync(payment.Id, default)).ReturnsAsync(payment);
        var payments = paymentsMock.Object;
        endpoint = Factory.Create<CancelPaymentEndpoint>(payments);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndIsSuccessful_ItShouldCancelPayment()
    {
        paymentMock.Setup(p => p.CancelPayment()).Returns(true);
        
        await endpoint.HandleAsync(new CancelPaymentRequest() { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.CancelPayment(), Times.Once);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Once);
        
        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndIsUnsuccessful_ItShouldNotCancelPayment()
    {
        paymentMock.Setup(p => p.CancelPayment()).Returns(false);
        
        await endpoint.HandleAsync(new CancelPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.CancelPayment(), Times.Once);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Never);

        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
    
    [Fact]
    public async Task WhenItIsCalledAndPaymentIsNonExisting_ItShouldReturn400()
    {
        paymentsMock
            .Setup(p => p.FindAndEnsureExistenceAsync(payment.Id, default))
            .Throws(new EntityDoesNotExistException());
        
        await endpoint.HandleAsync(new CancelPaymentRequest { Id = payment.Id}, default);
        
        paymentsMock.Verify(e => e.FindAndEnsureExistenceAsync(payment.Id, default), Times.Once);
        paymentMock.Verify(e => e.CancelPayment(), Times.Never);
        paymentsMock.Verify(e => e.Update(It.IsAny<Payment>()), Times.Never);

        endpoint.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
