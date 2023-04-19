namespace TicketManager.PaymentService.Domain.Payments;

public partial class Payment
{
    public static Payment CreateForTests(PaymentStatus status, DateTime dateCreated)
    {
        return new Payment
        {
            PaymentStatus = status,
            DateCreated = dateCreated
        };
    } 
}
