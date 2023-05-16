using FastEndpoints;
using TicketManager.Core.Contracts.Common;

namespace TicketManager.Core.Contracts.Events;

public class DeleteEventRequest : IEventRelated
{
    [FromClaim(Claims.AccountId)]
    public Guid AccountId { get; set; }
    public Guid Id { get; set; }

    public static class ErrorCodes
    {
        public const int EventDoesNotExist = 1;
    }
}
