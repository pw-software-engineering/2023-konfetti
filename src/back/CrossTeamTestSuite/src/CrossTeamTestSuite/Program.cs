using System.Text.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Contracts.Common;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;
using CrossTeamTestSuite.Tests.Examples;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite;

class Program
{
    public async static Task Main(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Wrong number of arguments provided");
        }

        var address = args[0];
        var adminEmail = args[1];
        var adminPassword = args[2];
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Tested app is at {address}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Admin email is {adminEmail}");
        Console.WriteLine($"Admin password is {adminPassword}");

        Console.ForegroundColor = ConsoleColor.White;
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(address);
        var adminLogin = await httpClient.CallEndpointAsync<AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest()
        {
            Email = adminEmail,
            Password = adminPassword,
        });
        var json = JsonSerializer.Serialize(adminLogin);
        Console.WriteLine(json);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {adminLogin!.AccessToken}");
        
        var organizers = await httpClient.CallEndpointAsync<OrganizerListRequest, PaginatedResponse<OrganizerDto>>(new OrganizerListRequest()
        {
            PageNumber = 0,
            PageSize = 10,
        });
        json = JsonSerializer.Serialize(organizers);
        Console.WriteLine(json);

        await new TestPipeline()
            .AddMultiTest(new SumTest())
            .GetExecutor()
            .ExecuteAsync();
    }
}

