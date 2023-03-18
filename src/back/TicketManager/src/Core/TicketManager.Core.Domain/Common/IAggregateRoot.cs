namespace TicketManager.Core.Domain.Common;

public interface IAggregateRoot<TId>
{
    public TId Id { get; }
}
