using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public class ListEventsRequest : IPaginatedRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
