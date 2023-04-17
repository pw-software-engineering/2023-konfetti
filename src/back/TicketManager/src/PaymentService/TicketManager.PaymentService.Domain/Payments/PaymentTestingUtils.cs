namespace TicketManager.PaymentService.Domain.Payments;

public partial class Payment
{
    public static Payment CreateForTests(PaymentStatus status, DateTime dateCreated, TimeSpan timeToExpire)
    {
        return new Payment
        {
            PaymentStatus = status,
            DateCreated = dateCreated,
            timeToExpire = timeToExpire    
        };
    } 
}