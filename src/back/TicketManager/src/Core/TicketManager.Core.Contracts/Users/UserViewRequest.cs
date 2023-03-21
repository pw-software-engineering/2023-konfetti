using FastEndpoints;

namespace TicketManager.Core.Contracts.Users;

public class UserViewRequest
{
    [FromClaim(ClaimType = "AccountId")]
    public Guid AccountId { get; set; }
}
