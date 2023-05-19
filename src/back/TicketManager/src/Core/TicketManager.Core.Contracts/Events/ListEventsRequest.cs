using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public class ListEventsRequest: IEventListRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool ShowAscending { get; set; }
    public EventListSortByDto SortBy { get; set; }
    public string? Location { get; set; }
    public string? EventNameFilter { get; set; }
    public DateTime? EarlierThanInclusiveFilter { get; set; }
    public DateTime? LaterThanInclusiveFilter { get; set; }
    public List<EventStatusDto>? EventStatusesFilter { get; set; }
}
