using System.Text.Json;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Extensions;

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

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(address);
        var response = await httpClient.CallEndpointAsync<AccountLoginRequest, AccountLoginResponse>(new AccountLoginRequest()
        {
            Email = adminEmail,
            Password = adminPassword,
        });
        var json = JsonSerializer.Serialize(response);
        Console.WriteLine(json);
    }
}

