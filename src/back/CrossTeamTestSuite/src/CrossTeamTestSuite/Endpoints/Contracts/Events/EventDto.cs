namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public EventStatusDto Status { get; set; }
    public List<SectorDto> Sectors { get; set; }
}
