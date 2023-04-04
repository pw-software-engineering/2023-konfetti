using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Organizers;

public class OrganizerViewRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid AccountId { get; set; }
}
