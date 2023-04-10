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
    }
}