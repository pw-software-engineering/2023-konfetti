using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Organizers;

public class OrganizerLoginTests : TestBase
{
    [Fact]
    public async Task Unverified_organizer_cannot_login()
    {
        var password = "Password1";
        var email = "oragnizer2@email.com";
        await RegisterOrganizer(email, password);

        await AnonymousClient.PostFailureAsync<AccountLoginEndpoint, AccountLoginRequest>(new()
        {
            Email = email,
            Password = password,
        });
    }

    [Fact]
    public async Task Negatively_verified_organizer_cannot_login()
    {
        var password = "Password1";
        var email = "oragnizer2@email.com";
        await RegisterOrganizer(email, password);

        var organizers = await AdminClient.GetSuccessAsync<OrganizerListEndpoint, OrganizerListRequest, PaginatedResponse<OrganizerDto>>(new()
        {
            PageNumber = 0,
            PageSize = 10,
        });

        var organizerToDecide = organizers.Items.First(o => o.Email == email);
        await AdminClient.PostSuccessAsync<OrganizerDecideEndpoint, OrganizerDecideRequest>(new()
        {
            OrganizerId = organizerToDecide.Id,
            IsAccepted = false,
        });
        
        await AnonymousClient.PostFailureAsync<AccountLoginEndpoint, AccountLoginRequest>(new()
        {
            Email = email,
            Password = password,
        });
    }
    
    private async Task RegisterOrganizer(string email, string password)
    {
        await AnonymousClient.PostSuccessAsync<RegisterOrganizerEndpoint, RegisterOrganizerRequest>(new()
        {
            Email = email,
            Password = password,
            Address = "address",
            CompanyName = "company name",
            DisplayName = "display name",
            PhoneNumber = "000000000",
            TaxId = "000000000",
            TaxIdType = TaxIdTypeDto.Pesel,
        });
    }
}
