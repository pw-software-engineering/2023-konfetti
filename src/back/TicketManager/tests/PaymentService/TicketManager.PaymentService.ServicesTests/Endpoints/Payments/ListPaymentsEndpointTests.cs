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
using TicketManager.PaymentService.ServicesTests.Helpers;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class ListPaymentsEndpointTests
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldListAllPayments()
    {
        
        var payments = new List<Payment> {new(), new(), new()};
        var paymentDtos = payments.Select(ToDtoConverters.PaymentToDto).ToList();
        
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
        var dbContext = dbContextMock.Object;
        var config = new PaymentServiceConfiguration("ApiKey");
        var endpoint = Factory.Create<ListPaymentsEndpoint>(dbContext, config);
        await endpoint.HandleAsync(default);
        var result = endpoint.Response;
        result.Should().BeEquivalentTo(paymentDtos);
    }
}
