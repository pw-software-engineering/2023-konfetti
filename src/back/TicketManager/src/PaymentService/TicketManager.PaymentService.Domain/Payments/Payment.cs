using TicketManager.PaymentService.Domain.Common;

namespace TicketManager.PaymentService.Domain.Payments;

public class Payment: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public PaymentStatus PaymentStatus { get; private set; }
    public DateTime DateCreated { get; private init; }
    public DateTime DateToExpire { get; private init; }
    private const int ExpirationTimeInMinutes = 30;
    public DateTime DateModified { get; set; }

    public Payment()
    {
        Id = Guid.NewGuid();
        PaymentStatus = PaymentStatus.Created;
        DateCreated = DateTime.UtcNow;
        DateToExpire = DateCreated.AddMinutes(ExpirationTimeInMinutes);
    }

    public virtual bool ConfirmPayment()
    {
        if (PaymentAlreadyDecidedOrExpired())
        {
            return false;
        }
        PaymentStatus = PaymentStatus.Confirmed;
        return true;
    }
    
    public virtual bool CancelPayment()
    {
        if (PaymentAlreadyDecidedOrExpired())
        {
            return false;
        }
        PaymentStatus = PaymentStatus.Cancelled;
        return true;
    }

    private bool PaymentAlreadyDecidedOrExpired()
    {
        var hasExpired = DateTime.UtcNow > DateToExpire;
        if (PaymentStatus is PaymentStatus.Created && hasExpired)
        {
            PaymentStatus = PaymentStatus.Expired;
        }
        return PaymentStatus is PaymentStatus.Expired or PaymentStatus.Confirmed or PaymentStatus.Cancelled;
    }
}

public enum PaymentStatus
{
    Created = 0,
    Confirmed = 1,
    Cancelled = 2,
    Expired = 3
}
