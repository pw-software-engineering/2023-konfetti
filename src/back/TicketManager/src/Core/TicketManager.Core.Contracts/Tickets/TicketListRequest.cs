using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Tickets;

public class TicketListRequest : IPaginatedRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid AccountId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
