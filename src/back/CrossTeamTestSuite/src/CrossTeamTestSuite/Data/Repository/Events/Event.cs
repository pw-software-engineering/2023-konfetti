namespace CrossTeamTestSuite.Data.Repository.Events;

public class Event
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public List<Sector> Sectors { get; set; }
}
