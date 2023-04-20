using FluentAssertions;
using TicketManager.PaymentService.Domain.Payments;
using Xunit;

namespace TicketManager.PaymentService.DomainTests.Payments;

public class PaymentTests
{
    public class ConstructorTests
    {
        [Fact]
        public void WhenPaymentCreated_ItShouldHaveStatusCreated()
        {
            var payment = new Payment();

            payment.PaymentStatus.Should().Be(PaymentStatus.Created);
        }
        
        [Fact]
        public void WhenConstructorCalledTwice_ItShouldReturnTwoDifferentIds()
        {
            var payment1 = new Payment();
            var payment2 = new Payment();

            payment1.Id.Should().NotBe(payment2.Id);
        }
        
        [Fact]
        public void WhenPaymentIsConfirmed_ItsStatusChangesToConfirmed()
        {
            var payment = new Payment();

            payment.ConfirmPayment();

            payment.PaymentStatus.Should().Be(PaymentStatus.Confirmed);
        }
        
        [Fact]
        public void WhenPaymentIsCancelled_ItsStatusChangesToCancelled()
        {
            var payment = new Payment();

            payment.CancelPayment();
            
            payment.PaymentStatus.Should().Be(PaymentStatus.Cancelled);
        }

        [Fact]
        public void WhenPaymentIsAlreadyDecided_DecisionShouldThrowException()
        {
            var payment1 = new Payment();
            var payment2 = new Payment();

            payment1.ConfirmPayment();
            payment2.CancelPayment();

            var func1 = () => payment1.CancelPayment();
            var func2 = () =>  payment2.ConfirmPayment();
            var func3 = () =>  payment1.ConfirmPayment();
            var func4 = () =>  payment2.CancelPayment();

            func1.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
            func2.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
            func3.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
            func4.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
        }
        
        [Fact]
        public void WhenPaymentHasAlreadyExpired_DecisionShouldThrowException()
        {
            var payment = Payment.CreateForTests(PaymentStatus.Created, DateTime.UtcNow.AddDays(-1));

            var func1 = () => payment.CancelPayment();
            var func2 = () =>  payment.ConfirmPayment();

            func1.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
            func2.Should().Throw<PaymentAlreadyDecidedOrExpiredException>();
        }
    }
}
