using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Data.Repository.Organizers;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;

namespace CrossTeamTestSuite.Endpoints.Instances.Organizers;

public class OrganizerRegisterInstance: EndpointInstance<OrganizerRegisterRequest>
{
    public override async Task HandleEndpointAsync(OrganizerRegisterRequest request)
    {
        await HttpClient.CallEndpointSuccessAsync(request);

        var dataAccess = DataAccessSingleton.GetInstance();
        var organizer = new Organizer(request.CompanyName, request.Address, (TaxIdType)request.TaxIdType, request.TaxId,
            request.DisplayName, request.Email, request.PhoneNumber);
        dataAccess.OrganizerRepository.AddOrganizer(organizer);
    }
}
