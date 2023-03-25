using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Users;

public class UserViewRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid AccountId { get; set; }
}
