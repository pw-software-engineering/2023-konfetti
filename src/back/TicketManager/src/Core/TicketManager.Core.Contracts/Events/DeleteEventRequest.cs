namespace TicketManager.Core.Contracts.Events;

public class DeleteEventRequest
{
    public Guid Id { get; set; }

    public static class ErrorCodes
    {
        public const int EventDoesNotExist = 1;
    }
}
