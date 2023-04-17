using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using TicketManager.Core.ServicesTests.Helpers;
using TicketManager.PaymentService.Contracts.Payments;
using TicketManager.PaymentService.Domain.Payments;
using TicketManager.PaymentService.Services.DataAccess;
using TicketManager.PaymentService.Services.Endpoints.Payments;
using TicketManager.PaymentService.Services.Services.Mockables;
using Xunit;

namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;

public class CheckPaymentStatusValidatorTest
{
    private Payment payment;
    private CheckPaymentStatusRequest request;
    private CheckPaymentStatusValidator validator;
    
    public CheckPaymentStatusValidatorTest()
    {
        payment = new Payment();
        request = new CheckPaymentStatusRequest { Id = payment.Id };
        validator = GetValidator(new List<Payment> { payment });
    }

    [Fact]
    public async Task WhenValidRequestIsProvided_ItShouldReturnTrue()
    {
        var result = await validator.ValidateAsync(request);
        result.EnsureCorrectness();
    }

    [Fact]
    public async Task WhenIdIsNotInDatabase_ItShouldReturnFalseWithPaymentNotInDatabaseErrorCode()
    {
        request.Id = Guid.Empty;
        var result = await validator.ValidateAsync(request);
        result.EnsureCorrectError(CheckPaymentStatusRequest.ErrorCodes.PaymentDoesNotExist);
    }
    private static CheckPaymentStatusValidator GetValidator(List<Payment> payments)
    {
        var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
        var scopeFactoryMock = new Mock<IServiceScopeFactory>();
        var dbResolverMock = new Mock<MockablePaymentDbResolver>();
        dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
        dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);

        var validator = new CheckPaymentStatusValidator(scopeFactoryMock.Object, dbResolverMock.Object);
        return validator;
    }
}