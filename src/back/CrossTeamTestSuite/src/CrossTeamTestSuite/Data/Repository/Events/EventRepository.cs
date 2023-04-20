namespace CrossTeamTestSuite.Data.Repository.Events;

public class EventRepository
{
    private List<Event> events = new();
    public string DefaultName => "Company name: don't copy";
    public Event? DefaultEvent => events.FirstOrDefault(e => e.Name == DefaultName);
    public IReadOnlyList<Event> Events => events.AsReadOnly();
    
    public void AddEvent(Event @event)
    {
        events.Add(@event);
    }
}
