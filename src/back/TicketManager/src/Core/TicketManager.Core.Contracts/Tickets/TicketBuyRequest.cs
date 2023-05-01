using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Tickets;

public class TicketBuyRequest
{
    [FromClaim(ClaimType = Claims.AccountId)]
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string SectorName { get; set; }
    public int NumberOfSeats { get; set; }
    
    public static class ErrorCodes
    {
        public const int EventDoesNotExist = 1;
        public const int SectorDoesNotExist = 2;
        public const int NumberOfSeatsIsNotPositive = 3;
    }
}
