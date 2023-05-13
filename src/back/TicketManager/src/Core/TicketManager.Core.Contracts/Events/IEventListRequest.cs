using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public interface IEventListRequest: IPaginatedRequest, ISortedRequest<EventListSortByDto>
{
    public string? Location { get; set; }
    public string? EventNameFilter { get; set; }
    public DateTime? EarlierThanInclusiveFilter { get; set; }
    public DateTime? LaterThanInclusiveFilter { get; set; }
    public List<EventStatusDto>? EventStatusesFilter { get; set; }
}
