namespace TicketManager.Core.Domain.Events;

public enum EventStatus
{
    Unverified = 0,
    Verified = 1,
    Published = 2,
    Opened = 3,
    Closed = 4,
    Finished = 5,
    Cancelled = 6,
    Held = 7,
    Recalled = 8,
}
