using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class ListPaymentsEndpointTests
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldAddPayment()
    {
        
        List<Payment> payments = new List<Payment> {new(), new(), new()};
        
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
        var dbContext = dbContextMock.Object;
        var endpoint = Factory.Create<ListPaymentsEndpoint>(dbContext);
        await endpoint.HandleAsync(default);
        var result = endpoint.Response;
        result.Payments.Count.Should().Be(payments.Count);
    }
}