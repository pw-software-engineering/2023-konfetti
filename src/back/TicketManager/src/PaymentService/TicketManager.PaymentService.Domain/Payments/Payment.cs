using TicketManager.PaymentService.Domain.Common;

namespace TicketManager.PaymentService.Domain.Payments;

public partial class Payment: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    private readonly TimeSpan  timeToExpire;
    
    public Guid Id { get; private init; }
    public PaymentStatus PaymentStatus { get; private set; }
    public DateTime DateCreated { get; private init; }
    public DateTime DateModified { get; set; }

    public bool HasExpired => DateCreated + timeToExpire <= DateTime.UtcNow;
    public bool IsDecided => PaymentStatus is PaymentStatus.Confirmed or PaymentStatus.Cancelled;

    public Payment()
    {
        Id = Guid.NewGuid();
        PaymentStatus = PaymentStatus.Created;
        DateCreated = DateTime.UtcNow;
        timeToExpire = TimeSpan.FromMinutes(30);
    }

    public void ConfirmPayment()
    {
        if (IsDecided || HasExpired)
        {
            throw new PaymentAlreadyDecidedOrExpiredException();
        }
        PaymentStatus = PaymentStatus.Confirmed;
    }
    
    public void CancelPayment()
    {
        if (IsDecided || HasExpired)
        {
            throw new PaymentAlreadyDecidedOrExpiredException();
        }
        PaymentStatus = PaymentStatus.Cancelled;
    }
}

public enum PaymentStatus
{
    Created = 0,
    Confirmed = 1,
    Cancelled = 2
}
