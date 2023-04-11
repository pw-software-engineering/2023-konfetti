namespace TicketManager.PaymentService.Domain.Common;

public interface IAggregateRoot<TId>
{
    public TId Id { get; }
}
