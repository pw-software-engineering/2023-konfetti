using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public class AddEventToFavoritesRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid AccountId { get; set; }
    
    public Guid EventId { get; set; }

    public static class ErrorCodes
    {
        public const int EventDoesNotExist = 1;
        public const int EventAlreadyAddedToFavorites = 2;
    }
}
