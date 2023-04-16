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
        public void WhenPaymentIsAlreadyDecided_ItIsNotPossibleToChangeStatus()
        {
            var payment1 = new Payment();
            var payment2 = new Payment();

            var result1 = payment1.ConfirmPayment();
            var result2 = payment2.CancelPayment();

            result1.Should().Be(true);
            result2.Should().Be(true);

            var result3 = payment1.CancelPayment();
            var result4 = payment2.ConfirmPayment();
            var result5 = payment1.CancelPayment();
            var result6 = payment2.ConfirmPayment();

            result3.Should().Be(false);
            result4.Should().Be(false);
            result5.Should().Be(false);
            result6.Should().Be(false);
        }
    }
}
