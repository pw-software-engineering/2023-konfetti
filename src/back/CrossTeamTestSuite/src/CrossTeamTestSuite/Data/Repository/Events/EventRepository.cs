namespace CrossTeamTestSuite.Data.Repository.Events;

public class EventRepository
{
    private List<Event> events = new();
    public string DefaultName => "Company name: don't copy";
    public IReadOnlyList<Event> Events => events.AsReadOnly();
    
    public void AddEvent(Event @event)
    {
        events.Add(@event);
    }
}
