namespace TicketManager.Core.Domain.Common;

public interface IIdentifiable<TId>
{
    public TId Id { get; }
}
