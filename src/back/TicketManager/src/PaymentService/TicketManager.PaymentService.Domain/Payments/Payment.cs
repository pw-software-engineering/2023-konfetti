using TicketManager.PaymentService.Domain.Common;

namespace TicketManager.PaymentService.Domain.Payments;

public class Payment: IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public PaymentStatus PaymentStatus { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime DateModified { get; set; }

    public Payment()
    {
        Id = Guid.NewGuid();
        PaymentStatus = PaymentStatus.Created;
        DateCreated = DateTime.UtcNow;
    }

}

public enum PaymentStatus
{
    Created,
    Confirmed,
    Cancelled,
    Expired
}

