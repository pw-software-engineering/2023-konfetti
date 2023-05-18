namespace CrossTeamTestSuite.Data.Repository.Events;

public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public EventStatus Status { get; set; }
    public List<Sector> Sectors { get; set; }
}

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
    Recalled = 8
}
