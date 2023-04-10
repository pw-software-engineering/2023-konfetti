using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public class ListEventForOrganizerRequest: IPaginatedRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid OrganizerId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
