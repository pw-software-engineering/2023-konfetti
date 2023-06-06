using CrossTeamTestSuite.Endpoints.Contracts.Tickets;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Tickets;

public class TicketBuyInstance: EndpointInstance<TicketBuyRequest, TicketBuyResponse>
{
    public override async Task<TicketBuyResponse?> HandleEndpointAsync(TicketBuyRequest request)
    {
        return await HttpClient.CallEndpointSuccessAsync<TicketBuyRequest, TicketBuyResponse>(request);
    }
}
