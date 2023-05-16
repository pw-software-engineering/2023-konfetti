namespace TicketManager.Core.Contracts.Events;

public class EventDecideRequest
{
    public Guid Id { get; set; }
    public bool IsAccepted { get; set; }
}
