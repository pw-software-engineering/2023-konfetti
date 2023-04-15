using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.Configuration;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class CheckPaymentStatusEndpointTest
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldAddPayment()
    {
        var payments = new List<Payment> {new(), new(), new()};
        
        
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
        var dbContext = dbContextMock.Object;
        var config = new PaymentServiceConfiguration("ApiKey");
        var endpoint = Factory.Create<CheckPaymentStatusEndpoint>(dbContext, config);

        var secondPayment = payments[1];
        var request = new CheckPaymentStatusRequest { Id = secondPayment.Id };
        var expected = secondPayment.PaymentStatus;
        
        await endpoint.HandleAsync(request, default);
        var result = endpoint.Response;
        
        result.Status.Should().Be((PaymentStatusDto)expected);
    }
}
