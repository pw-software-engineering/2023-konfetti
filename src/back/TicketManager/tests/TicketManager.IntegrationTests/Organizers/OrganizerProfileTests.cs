using FluentAssertions;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests.Organizers;

public class OrganizerProfileTests : TestBase
{
    [Fact]
    public async Task Organizer_can_manage_its_profile()
    {
        await VerifyProfileAsync();

        await EditEmailAsync(DefaultOrganizer.Email);
        await VerifyProfileAsync();

        await EditEmailAsync("anotherorganizer@email.com");
        await VerifyProfileAsync();

        await EditCompanyNameAsync("new company name");
        await VerifyProfileAsync();

        await EditAddressAsync("new address");
        await VerifyProfileAsync();

        await EditTaxIdAsync("new tax id");
        await VerifyProfileAsync();

        await EditTaxIdTypeAsync(TaxIdTypeDto.Regon);
        await VerifyProfileAsync();

        await EditDisplayNameAsync("new display name");
        await VerifyProfileAsync();

        await EditPhoneNumberAsync("new phone number");
        await VerifyProfileAsync();
    }

    private async Task EditEmailAsync(string email)
    {
        DefaultOrganizer.Email = email;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            Email = email,
        });
    }
    
    private async Task EditCompanyNameAsync(string companyName)
    {
        DefaultOrganizer.CompanyName = companyName;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            CompanyName = companyName,
        });
    }
    
    private async Task EditAddressAsync(string address)
    {
        DefaultOrganizer.Address = address;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            Address = address,
        });
    }
    
    private async Task EditTaxIdAsync(string taxId)
    {
        DefaultOrganizer.TaxId = taxId;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            TaxId = taxId,
        });
    }
    
    private async Task EditTaxIdTypeAsync(TaxIdTypeDto taxIdType)
    {
        DefaultOrganizer.TaxIdType = taxIdType;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            TaxIdType = taxIdType,
        });
    }
    
    private async Task EditDisplayNameAsync(string displayName)
    {
        DefaultOrganizer.DisplayName = displayName;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            DisplayName = displayName,
        });
    }
    
    private async Task EditPhoneNumberAsync(string phoneNumber)
    {
        DefaultOrganizer.PhoneNumber = phoneNumber;

        await OrganizerClient.PostSuccessAsync<UpdateOrganizerEndpoint, UpdateOrganizerRequest>(new()
        {
            PhoneNumber = phoneNumber,
        });
    }

    private async Task VerifyProfileAsync()
    {
        var profile = await OrganizerClient.GetSuccessAsync<OrganizerViewEndpoint, OrganizerViewRequest, OrganizerDto>(new());

        profile.Should().BeEquivalentTo(DefaultOrganizer);
    }
}
