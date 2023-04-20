using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Data.Repository.Events;
using CrossTeamTestSuite.Endpoints.Contracts.Common;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Events;

public class EventCreateInstance: EndpointInstance<EventCreateRequest, IdResponse>
{
    public override async Task<IdResponse?> HandleEndpointAsync(EventCreateRequest request)
    {
        var response = await HttpClient.CallEndpointAsync<EventCreateRequest, IdResponse>(request);
        var eventRepository = DataAccessSingleton.GetInstance().EventRepository;
        var @event = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Location = request.Location,
            Date = request.Date,
            Sectors = request.Sectors.Select(x => new Sector
            {
                Name = x.Name,
                PriceInSmallestUnit = x.PriceInSmallestUnit,
                NumberOfColumns = x.NumberOfColumns,
                NumberOfRows = x.NumberOfRows
            }).ToList()
        };
        eventRepository.AddEvent(@event);
        return response;
    }
}
