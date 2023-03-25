using FastEndpoints;
using TicketManager.Core.Contracts.Accounts;
using TicketManager.Core.Contracts.Organizers;
using TicketManager.Core.Contracts.Users;
using TicketManager.Core.Services.Endpoints.Accounts;
using TicketManager.Core.Services.Endpoints.Organizers;
using TicketManager.Core.Services.Endpoints.Users;
using Xunit;

namespace TicketManager.IntegrationTests;


public class TestBase : IAsyncLifetime
{
    private readonly TicketManagerApp app;
    protected readonly HttpClient AnonymousClient;
    protected readonly HttpClient UserClient;
    protected readonly HttpClient OrganizerClient;

    protected readonly UserDto DefaultUser;

    public TestBase()
    {
        app = new();
        AnonymousClient = app.CreateClient();
        
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
            Password = "Password1",
            FirstName = DefaultUser.FirstName,
            LastName = DefaultUser.LastName,
            BirthDate = DefaultUser.BirthDate,
        }).Wait();
        var userLoginTask = UserClient.POSTAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest
        {
            Email = DefaultUser.Email,
            Password = "Password1",
        });
        userLoginTask.Wait();
        var token = userLoginTask.Result.Result!.AccessToken;
        UserClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var userIdTask = UserClient.GETAsync<UserViewEndpoint, UserViewRequest, UserDto>(new());
        userIdTask.Wait();
        DefaultUser.Id = userIdTask.Result.Result!.Id;
        
        OrganizerClient = app.CreateClient();
        OrganizerClient.POSTAsync<RegisterOrganizerEndpoint, RegisterOrganizerRequest>(new RegisterOrganizerRequest
        {
            Email = "organizer@organizer.com",
            Password = "Password1",
            Address = "address",
            CompanyName = "companyname",
            DisplayName = "displayname",
            PhoneNumber = "123456789",
            TaxId = "000000000",
            TaxIdType = TaxIdTypeDto.Pesel,
        }).Wait();
        var organizerLoginTask = OrganizerClient.POSTAsync<AccountLoginEndpoint, AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest
        {
            Email = "organizer@organizer.com",
            Password = "Password1",
        });
        organizerLoginTask.Wait();
        token = organizerLoginTask.Result.Result!.AccessToken;
        OrganizerClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    public async Task InitializeAsync()
    {
        await app.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await app.DisposeAsync();
    }
}
