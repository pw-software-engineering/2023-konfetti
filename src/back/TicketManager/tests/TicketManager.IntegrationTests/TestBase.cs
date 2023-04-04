using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.DataAccess.Repositories;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Endpoints.Users;
using TicketManager.Core.Services.Services.PasswordManagers;
using TicketManager.IntegrationTests.Extensions;
using Xunit;

namespace TicketManager.IntegrationTests;


public class TestBase : IAsyncDisposable
{
    private readonly TicketManagerApp app;
    protected readonly HttpClient AnonymousClient;
    protected readonly HttpClient UserClient;
    protected readonly HttpClient OrganizerClient;
    protected readonly HttpClient AdminClient;

    protected readonly UserDto DefaultUser;
    protected readonly OrganizerDto DefaultOrganizer;

    public TestBase()
    {
        app = new();
        app.InitializeAsync().Wait();
        AnonymousClient = app.CreateClient();

        var password = "Password1";
        
        UserClient = app.CreateClient();
        DefaultUser = new()
        {
            Email = "user@user.com",
            FirstName = "name",
            LastName = "lastname",
            BirthDate = new DateOnly(2000, 1, 1),
        };
        UserClient.POSTAsync<RegisterUserEndpoint, RegisterUserRequest>(new RegisterUserRequest
        {
            Email = DefaultUser.Email,
            Password = password,
            FirstName = DefaultUser.FirstName,
            LastName = DefaultUser.LastName,
            BirthDate = DefaultUser.BirthDate,
        }).Wait();
        var userLoginTask = UserClient.POSTAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest
        {
            Email = DefaultUser.Email,
            Password = password,
        });
        userLoginTask.Wait();
        var token = userLoginTask.Result.Result!.AccessToken;
        UserClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var userIdTask = UserClient.GETAsync<UserViewEndpoint, UserViewRequest, UserDto>(new());
        userIdTask.Wait();
        DefaultUser.Id = userIdTask.Result.Result!.Id;

        AdminClient = app.CreateClient();
        
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

        var accounts = new Repository<Account, Guid>(dbContext);
        var adminAccount = dbContext.Accounts.AsTracking().First(a => a.Role == AccountRoles.Admin);
        adminAccount.SetPassword(new PasswordManager().GetHash(password));
        accounts.UpdateAsync(adminAccount, default).Wait();

        var adminLoginTask = AdminClient.PostSuccessAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new()
        {
            Email = "admin@email.com",
            Password = password,
        });
        adminLoginTask.Wait();
        token = adminLoginTask.Result.AccessToken;
        AdminClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        OrganizerClient = app.CreateClient();
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
        OrganizerClient.POSTAsync<RegisterOrganizerEndpoint, RegisterOrganizerRequest>(new RegisterOrganizerRequest
        {
            Email = DefaultOrganizer.Email,
            Password = password,
            Address = DefaultOrganizer.Address,
            CompanyName = DefaultOrganizer.CompanyName,
            DisplayName = DefaultOrganizer.DisplayName,
            PhoneNumber = DefaultOrganizer.PhoneNumber,
            TaxId = DefaultOrganizer.TaxId,
            TaxIdType = TaxIdTypeDto.Pesel,
        }).Wait();
        // Authorize organizer to be able to login
        var repository = new Repository<Organizer, Guid>(dbContext);
        
        var dbOrganizer = dbContext.Organizers.AsTracking().First(o => o.Email == DefaultOrganizer.Email);
        dbOrganizer.Decide(true);
        repository.UpdateAsync(dbOrganizer, default).Wait();

        var organizerLoginTask = OrganizerClient.POSTAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest
        {
            Email = DefaultOrganizer.Email,
            Password = password,
        });
        organizerLoginTask.Wait();
        token = organizerLoginTask.Result.Result!.AccessToken;
        OrganizerClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var organizerIdTask = OrganizerClient.GETAsync<OrganizerViewEndpoint, OrganizerViewRequest, OrganizerDto>(new());
        organizerIdTask.Wait();
        DefaultOrganizer.Id = organizerIdTask.Result.Result!.Id;
    }

    public async ValueTask DisposeAsync()
    {
        await app.DisposeAsync();
    }
}
