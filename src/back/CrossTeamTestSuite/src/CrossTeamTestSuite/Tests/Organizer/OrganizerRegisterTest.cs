using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Instances.Organizers;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite.Tests.Organizer;

public class OrganizerRegisterTest: SingleTest
{
    public override string Name => "Organizer register test";
    public override async Task ExecuteAsync()
    {
        var dataAccess = DataAccessSingleton.GetInstance();
        var organizerRegisterRequest = new OrganizerRegisterRequest
        {
            Address = "Warsaw, Center. Poland",
            CompanyName = "Some company LTD",
            DisplayName = "Fun event company",
            Email = dataAccess.OrganizerRepository.DefaultEmail,
            Password = dataAccess.OrganizerRepository.DefaultPassword,
            PhoneNumber = "123456789",
            TaxId = "123456789",
            TaxIdType = TaxIdTypeDto.Nip
        };
        
        var organizerRegisterInstance = new OrganizerRegisterInstance();
        await organizerRegisterInstance.HandleEndpointAsync(organizerRegisterRequest);
    }
}
