using FastEndpoints;

namespace TicketManager.Core.Contracts.Organizers;

public class OrganizerViewRequest
{
    [FromClaim(ClaimType = "AccountId")]
    public Guid AccountId { get; set; }
}
