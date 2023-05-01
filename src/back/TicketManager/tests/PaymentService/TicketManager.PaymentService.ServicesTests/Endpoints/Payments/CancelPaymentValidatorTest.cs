// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using Moq.EntityFrameworkCore;
// using TicketManager.Core.ServicesTests.Helpers;
// using TicketManager.PaymentService.Contracts.Payments;
// using TicketManager.PaymentService.Domain.Payments;
// using TicketManager.PaymentService.Services.DataAccess;
// using TicketManager.PaymentService.Services.Endpoints.Payments;
// using TicketManager.PaymentService.Services.Services.Mockables;
// using Xunit;
//
// namespace TicketManager.PaymentService.ServicesTests.Endpoints.Payments;
//
// public class CancelPaymentValidatorTest
// {
//     private List<Payment> payments;
//     private CancelPaymentValidator validator;
//     
//     public CancelPaymentValidatorTest()
//     {
//         payments = new List<Payment>
//         {
//             Payment.CreateForTests(PaymentStatus.Created, DateTime.Now.AddDays(1)),
//             Payment.CreateForTests(PaymentStatus.Confirmed, DateTime.Now.AddDays(1)),
//             Payment.CreateForTests(PaymentStatus.Cancelled, DateTime.Now.AddDays(1)),
//             Payment.CreateForTests(PaymentStatus.Cancelled, DateTime.Now.AddDays(-1))
//         };
//         validator = GetValidator(payments);
//     }
//
//     [Fact]
//     public async Task WhenValidRequestIsProvided_ItShouldReturnTrue()
//     {
//         var payment = payments[0];
//         var request = new CancelPaymentRequest { Id = payment.Id };
//         var result = await validator.ValidateAsync(request);
//         result.EnsureCorrectness();
//     }
//
//     [Fact]
//     public async Task WhenIdIsNotInDatabase_ItShouldReturnFalseWithPaymentNotInDatabaseErrorCode()
//     {
//         var request = new CancelPaymentRequest { Id = Guid.Empty };
//         var result = await validator.ValidateAsync(request);
//         result.EnsureCorrectError(CancelPaymentRequest.ErrorCodes.PaymentDoesNotExist);
//     }
//
//     [Fact]
//     public async Task WhenPaymentAlreadyConfirmed_ItShouldReturnFalseWithPaymentAlreadyDecided()
//     {
//         var payment = payments[1];
//         var request = new CancelPaymentRequest { Id = payment.Id };
//         var result = await validator.ValidateAsync(request);
//         result.EnsureCorrectError(CancelPaymentRequest.ErrorCodes.PaymentAlreadyDecided);
//     }
//     
//     [Fact]
//     public async Task WhenPaymentAlreadyCancelled_ItShouldReturnFalseWithPaymentAlreadyDecided()
//     {
//         var payment = payments[2];
//         var request = new CancelPaymentRequest { Id = payment.Id };
//         var result = await validator.ValidateAsync(request);
//         result.EnsureCorrectError(CancelPaymentRequest.ErrorCodes.PaymentAlreadyDecided);
//     }
//     
//     [Fact]
//     public async Task WhenPaymentAlreadyExpired_ItShouldReturnFalseWithPaymentHasExpired()
//     {
//         var payment = payments[3];
//         var request = new CancelPaymentRequest() { Id = payment.Id };
//         var result = await validator.ValidateAsync(request);
//         result.EnsureCorrectError(CancelPaymentRequest.ErrorCodes.PaymentHasExpired);
//     }
//     private static CancelPaymentValidator GetValidator(List<Payment> payments)
//     {
//         var dbContextMock = new Mock<PaymentDbContext>(new DbContextOptionsBuilder<PaymentDbContext>().Options);
//         var scopeFactoryMock = new Mock<IServiceScopeFactory>();
//         var dbResolverMock = new Mock<MockablePaymentDbResolver>();
//         dbContextMock.Setup(d => d.Payments).ReturnsDbSet(payments);
//         dbResolverMock.Setup(r => r.Resolve(It.IsAny<IServiceScope>())).Returns(dbContextMock.Object);
//
//         var validator = new CancelPaymentValidator(scopeFactoryMock.Object, dbResolverMock.Object);
//         return validator;
//     }
// }
