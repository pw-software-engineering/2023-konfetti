namespace TicketManager.Core.Contracts.Events;

public class EventDecideRequest
{
    public Guid Id { get; set; }
    public bool IsAccepted { get; set; }
    
    public static class ErrorCodes
    {
        public const int EventDoesNotExist = 1;
        public const int EventIsAlreadyVerified = 2;
    }
}
