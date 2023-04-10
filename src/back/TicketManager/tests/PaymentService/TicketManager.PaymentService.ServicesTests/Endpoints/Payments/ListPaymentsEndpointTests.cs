using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class ListPaymentsEndpointTests
{
    [Fact]
    public async Task WhenItIsCalled_ItShouldListAllPayments()
    {
        
        List<Payment> payments = new List<Payment> {new(), new(), new()};
        List<PaymentDto> paymentDtos = payments.Select(PaymentToDto).ToList();
        
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
        var dbContext = dbContextMock.Object;
        var endpoint = Factory.Create<ListPaymentsEndpoint>(dbContext);
        await endpoint.HandleAsync(default);
        var result = endpoint.Response;
        result.Should().BeEquivalentTo(paymentDtos);
    }

    public PaymentDto PaymentToDto(Payment payment)
    {
        return new PaymentDto()
        {
            Token = payment.Id,
            PaymentStatus = (PaymentStatusDto)payment.PaymentStatus,
            DateCreated = payment.DateCreated
        };
    }
}
