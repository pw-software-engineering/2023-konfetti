using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.DataAccess.Repositories;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class CreatePaymentEndpointTests
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldAddPayment()
    {
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        var dbContext = dbContextMock.Object;
        var paymentsMock = new Mock<Repository<Payment, Guid>>(dbContext);
        var payments = paymentsMock.Object;
        var config = new PaymentServiceConfiguration("ApiKey");
        var endpoint = Factory.Create<CreatePaymentEndpoint>(payments, config);
        await endpoint.HandleAsync(default);
        
        paymentsMock.Verify(e => e.Add(It.IsAny<Payment>()), Times.Once);
    }
}
