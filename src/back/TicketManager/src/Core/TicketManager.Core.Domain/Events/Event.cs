using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Event : IAggregateRoot<Guid>, IOptimisticConcurrent
{
    public Guid Id { get; private init; }
    public Guid OrganizerId { get; private init; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Location { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public EventStatus Status { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public DateTime DateModified { get; set; }
    
    private Event() { }
    
    public Event(Guid organizerId, string name, string description, string location, DateTime date)
    {
        Id = Guid.NewGuid();
        OrganizerId = organizerId;
        Name = name;
        Description = description;
        Location = location;
        Date = date;
        Status = EventStatus.Verified;
    }

    public void ChangeEventStatus(EventStatus status)
    {
        switch(status)
        {
            case EventStatus.Verified:
                if(Status != EventStatus.Unverified &&  Status != EventStatus.Cancelled)
                {
                    throw new InvalidOperationException("Cannot change event status to Verified from current status.");
                }
                break;
            case EventStatus.Published:
                if(Status != EventStatus.Verified)
                {
                    throw new InvalidOperationException("Cannot change event status to Published from current status.");
                }
                break;
            case EventStatus.Opened:
                if (Status != EventStatus.Published && Status != EventStatus.Closed)
                {
                    throw new InvalidOperationException("Cannot change event status to Opened from current status.");
                }
                break;
            case EventStatus.Closed:
                if (Status != EventStatus.Opened)
                {
                    throw new InvalidOperationException("Cannot change event status to Closed from current status.");
                }
                break;
            case EventStatus.Finished:
                if (Status != EventStatus.Opened && Status != EventStatus.Closed)
                {
                    throw new InvalidOperationException("Cannot change event status to Finished from current status.");
                }
                break;
            case EventStatus.Cancelled:
                if(Status != EventStatus.Verified && Status != EventStatus.Published)
                {
                    throw new InvalidOperationException("Cannot change event status to Cancelled from current status.");
                }
                break;
            case EventStatus.Held:
                if(Status != EventStatus.Published && Status != EventStatus.Opened)
                {
                    throw new InvalidOperationException("Cannot change event status to Held from current status.");
                }
                break;
            case EventStatus.Recalled:
                if(Status != EventStatus.Held && Status != EventStatus.Opened)
                {
                    throw new InvalidOperationException("Cannot change event status to Recalled from current status.");
                }
                break;
            case EventStatus.Unverified:
            default:
                throw new InvalidOperationException("Cannot change event status to the specified value.");
        }

        Status = status;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void UpdateLocation(string location)
    {
        Location = location;
    }
    
    public void UpdateDate(DateTime date)
    {
        Date = date;
    }
}
