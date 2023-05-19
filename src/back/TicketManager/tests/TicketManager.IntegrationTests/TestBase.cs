using FastEndpoints;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Contracts.Common;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.PasswordManagers;
using TicketManager.IntegrationTests.Extensions;

namespace TicketManager.IntegrationTests;


public class TestBase : IAsyncDisposable
{
    private readonly TicketManagerApp coreApp;
    private readonly PaymentServiceApp paymentServiceApp;

    protected readonly HttpClient PaymentClient;
    
    protected readonly HttpClient AnonymousClient;
    protected readonly HttpClient UserClient;
    protected readonly HttpClient OrganizerClient;
    protected readonly HttpClient AdminClient;

    protected UserDto DefaultUser = null!;
    protected OrganizerDto DefaultOrganizer = null!;

    public TestBase()
    {
        paymentServiceApp = new();
        paymentServiceApp.InitializeAsync().Wait();
        PaymentClient = paymentServiceApp.CreateClient();
        PaymentClient.DefaultRequestHeaders.Add("pay_api_key", "ApiKey");
        
        coreApp = new(PaymentClient);
        coreApp.InitializeAsync().Wait();
        AnonymousClient = coreApp.CreateClient();

        var password = "Password1";
        
        UserClient = coreApp.CreateClient();
        ConfigureUserAsync(password).Wait();

        AdminClient = coreApp.CreateClient(); ;
        ConfigureAdminAsync(password).Wait();

        OrganizerClient = coreApp.CreateClient();
        ConfigureOrganizerAsync(password).Wait();
    }

    public async Task WaitForProcessingAsync()
    {
        var harness = coreApp.Services.GetTestHarness();
        var bus = harness.Bus;
        var activityMonitor = bus.CreateBusActivityMonitor();
        await activityMonitor.AwaitBusInactivity(TimeSpan.FromSeconds(15));
        var consumed = await harness.Consumed.Any();
        consumed.Should().BeTrue();
    }

    private async Task ConfigureOrganizerAsync(string password)
    {
        DefaultOrganizer = new()
        {
            Email = "organizer@organizer.com",
            Address = "address",
            CompanyName = "companyname",
            DisplayName = "displayname",
            PhoneNumber = "123456789",
            TaxId = "000000000",
            TaxIdType = TaxIdTypeDto.Pesel,
            VerificationStatus = VerificationStatusDto.VerifiedPositively,
        };
        await OrganizerClient.PostSuccessAsync<RegisterOrganizerEndpoint, RegisterOrganizerRequest>(new RegisterOrganizerRequest
        {
            Email = DefaultOrganizer.Email,
            Password = password,
            Address = DefaultOrganizer.Address,
            CompanyName = DefaultOrganizer.CompanyName,
            DisplayName = DefaultOrganizer.DisplayName,
            PhoneNumber = DefaultOrganizer.PhoneNumber,
            TaxId = DefaultOrganizer.TaxId,
            TaxIdType = TaxIdTypeDto.Pesel,
        });
        
        var organizers = await AdminClient.GetSuccessAsync<OrganizerListEndpoint, OrganizerListRequest, PaginatedResponse<OrganizerDto>>(new()
        {
            PageNumber = 0,
            PageSize = 10,
        });
        var organizerToDecide = organizers.Items.First(o => o.Email == DefaultOrganizer.Email);
        await AdminClient.PostSuccessAsync<OrganizerDecideEndpoint, OrganizerDecideRequest>(new()
        {
            OrganizerId = organizerToDecide.Id,
            IsAccepted = true,
        });

        var organizerLogin = await OrganizerClient
            .PostSuccessAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new()
            {
                Email = DefaultOrganizer.Email,
                Password = password,
            });
        var token = organizerLogin.AccessToken;
        OrganizerClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var organizerId = await OrganizerClient.GetSuccessAsync<OrganizerViewEndpoint, OrganizerViewRequest, OrganizerDto>(new());
        DefaultOrganizer.Id = organizerId.Id;
    }

    private async Task ConfigureAdminAsync(string password)
    {
        using var scope = coreApp.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

        var accounts = new Repository<Account, Guid>(dbContext);
        var adminAccount = dbContext.Accounts.AsTracking().First(a => a.Role == AccountRoles.Admin);
        adminAccount.SetPassword(new PasswordManager().GetHash(password));
        await accounts.UpdateAsync(adminAccount, default);

        var adminLogin = await AdminClient.PostSuccessAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new()
        {
            Email = "admin@email.com",
            Password = password,
        });
        var token = adminLogin.AccessToken;
        AdminClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    private async Task ConfigureUserAsync(string password)
    {
        DefaultUser = new()
        {
            Email = "user@user.com",
            FirstName = "name",
            LastName = "lastname",
            BirthDate = new DateOnly(2000, 1, 1),
        };
        await UserClient.POSTAsync<RegisterUserEndpoint, RegisterUserRequest>(new RegisterUserRequest
        {
            Email = DefaultUser.Email,
            Password = password,
            FirstName = DefaultUser.FirstName,
            LastName = DefaultUser.LastName,
            BirthDate = DefaultUser.BirthDate,
        });
        var userLogin = await UserClient.PostSuccessAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest
        {
            Email = DefaultUser.Email,
            Password = password,
        });
        var token = userLogin.AccessToken;
        UserClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var userId = await UserClient.GetSuccessAsync<UserViewEndpoint, UserViewRequest, UserDto>(new());
        DefaultUser.Id = userId.Id;
    }

    public async ValueTask DisposeAsync()
    {
        await coreApp.DisposeAsync();
        await paymentServiceApp.DisposeAsync();
    }
}
