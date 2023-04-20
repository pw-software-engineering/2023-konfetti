using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Instances.Organizers;
using CrossTeamTestSuite.TestsInfrastructure;
using FluentAssertions;

namespace CrossTeamTestSuite.Tests.Organizer;

public class OrganizerDecideTest: SingleTest
{
    public override string Name => "Organizer decide test";
    public override async Task ExecuteAsync()
    {
        var dataAccess = DataAccessSingleton.GetInstance();
        // Find organizer Id
        var organizerListRequest = new OrganizerListRequest
        {
            PageNumber = 0,
            PageSize = 100,
            ShowAscending = true,
            SortBy = OrganizerListSortByDto.Email,
            EmailFilter = dataAccess.OrganizerRepository.DefaultEmail
        };
        var organizerListInstance = new OrganizerListInstance();
        organizerListInstance.SetToken(CommonTokenType.AdminToken);

        var list = await organizerListInstance.HandleEndpointAsync(organizerListRequest);
        list.Should().NotBeNull();
        list!.Items.Should().HaveCount(1);

        var organizerDto = list!.Items.First();
        
        // Approve organizer
        var organizerDecideRequest = new OrganizerDecideRequest
        {
            OrganizerId = organizerDto.Id,
            IsAccepted = true
        };

        var organizerDecideInstance = new OrganizerDecideInstance();
        organizerDecideInstance.SetToken(CommonTokenType.AdminToken);
        await organizerDecideInstance.HandleEndpointAsync(organizerDecideRequest);
    }
}
