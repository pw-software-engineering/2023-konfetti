namespace TicketManager.Core.Domain.Common;

public interface IOptimisticConcurrent
{
    public DateTime DateModified { get; set; }
}
