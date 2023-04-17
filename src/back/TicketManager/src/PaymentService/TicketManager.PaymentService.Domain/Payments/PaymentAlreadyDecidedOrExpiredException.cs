namespace TicketManager.PaymentService.Domain.Payments;

public class PaymentAlreadyDecidedOrExpiredException: Exception
{
    public PaymentAlreadyDecidedOrExpiredException() : base("Payment already has been decided or has expired!") {}
}