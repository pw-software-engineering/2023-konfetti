using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Instances.Organizers;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Organizer;

public class OrganizerViewTest: SingleTest
{
    public override string Name => "Organizer view test";
    public override async Task ExecuteAsync()
    {
        var organizerViewRequest = new OrganizerViewRequest();
        var organizerViewInstance = new OrganizerViewInstance();
        organizerViewInstance.SetToken(CommonTokenType.OrganizerToken);
        var organizerDto = await organizerViewInstance.HandleEndpointAsync(organizerViewRequest);
        organizerDto.Should().NotBeNull();
        
        var dataAccess = DataAccessSingleton.GetInstance();
        var organizer = dataAccess.OrganizerRepository.DefaultAccount;
        
        organizerDto.Should().BeEquivalentTo(organizer);
    }
}
