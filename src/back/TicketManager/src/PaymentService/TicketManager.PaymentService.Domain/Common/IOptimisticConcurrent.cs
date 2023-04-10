namespace TicketManager.PaymentService.Domain.Common;

public interface IOptimisticConcurrent
{
    public DateTime DateModified { get; set; }
}
