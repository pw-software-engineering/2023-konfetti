using TicketManager.PaymentService.Domain.Common;

namespace TicketManager.PaymentService.Domain.Payments;

public class Payment: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    private readonly TimeSpan  TimeToExpire;
    
    public Guid Id { get; private init; }
    public PaymentStatus PaymentStatus { get; private set; }
    public DateTime DateCreated { get; private init; }
    public DateTime DateModified { get; set; }

    private bool HasExpired => DateCreated + TimeToExpire <= DateTime.UtcNow;

    public Payment()
    {
        Id = Guid.NewGuid();
        PaymentStatus = PaymentStatus.Created;
        DateCreated = DateTime.UtcNow;
        TimeToExpire = TimeSpan.FromMinutes(30);
    }

    public virtual void ConfirmPayment()
    {
        if (PaymentAlreadyDecidedOrExpired())
        {
            throw new PaymentAlreadyDecidedOrExpiredException();
        }
        PaymentStatus = PaymentStatus.Confirmed;
    }
    
    public virtual void CancelPayment()
    {
        if (PaymentAlreadyDecidedOrExpired())
        {
            throw new PaymentAlreadyDecidedOrExpiredException();
        }
        PaymentStatus = PaymentStatus.Cancelled;
    }

    private bool PaymentAlreadyDecidedOrExpired()
    {
        if (PaymentStatus is PaymentStatus.Created && HasExpired)
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
