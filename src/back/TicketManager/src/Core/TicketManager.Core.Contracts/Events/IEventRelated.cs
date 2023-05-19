namespace TicketManager.Core.Contracts.Events;

public interface IEventRelated
{
    public Guid Id { get; }
    public Guid AccountId { get; }
}
