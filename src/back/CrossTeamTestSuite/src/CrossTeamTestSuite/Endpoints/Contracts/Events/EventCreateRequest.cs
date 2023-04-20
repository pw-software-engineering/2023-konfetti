using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;
using CrossTeamTestSuite.Endpoints.Contracts.Common;

namespace CrossTeamTestSuite.Endpoints.Contracts.Events;

public class EventCreateRequest: IRequest<IdResponse>
{
    [JsonIgnore]
    public string Path => "/event/create";
    [JsonIgnore] 
    public RequestType Type => RequestType.Post;
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public List<SectorDto> Sectors { get; set; }
}
