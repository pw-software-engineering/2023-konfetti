using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Events;
using CrossTeamTestSuite.Endpoints.Instances.Events;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Event;

public class EventCreateTest: SingleTest
{
    public override string Name => "Event create test";
    public override async Task ExecuteAsync()
    {
        var eventCreateRequest = new EventCreateRequest
        {
            Name = DataAccessSingleton.GetInstance().EventRepository.DefaultName,
            Description = "Some veeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeery long description 1234=!@#$%^&*()_+",
            Date = DateTime.Now.AddMonths(3),
            Location = "Warsaw, Central",
            Sectors = new List<SectorDto>
            {
                new()
                {
                    Name = "VIP",
                    PriceInSmallestUnit = 1000,
                    NumberOfColumns = 10,
                    NumberOfRows = 10
                },
                new()
                {
                    Name = "Regular",
                    PriceInSmallestUnit = 500,
                    NumberOfColumns = 10,
                    NumberOfRows = 10
                }
            }
        };
        var eventCreateInstance = new EventCreateInstance();
        eventCreateInstance.SetToken(CommonTokenType.OrganizerToken);

        var response = await eventCreateInstance.HandleEndpointAsync(eventCreateRequest);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
    }
}
