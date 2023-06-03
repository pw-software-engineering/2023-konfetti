using System.Text.Json.Serialization;
using CrossTeamTestSuite.Endpoints.Contracts.Abstraction;

namespace CrossTeamTestSuite.Endpoints.Contracts.Tickets;

public class TicketBuyRequest: IRequest<TicketBuyResponse>
{
    [JsonIgnore]
    public string Path => "/ticket/buy";
    [JsonIgnore]
    public RequestType Type => RequestType.Post;
    public Guid EventId { get; set; }
    public string SectorName { get; set; }
    public int NumberOfSeats { get; set; }
}
