namespace TicketManager.Core.Contracts.Common;

public interface IPaginatedRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
