using System.Text.Json;
using CrossTeamTestSuite.Data;
using CrossTeamTestSuite.Endpoints.Contracts.Accounts;
using CrossTeamTestSuite.Endpoints.Contracts.Common;
using CrossTeamTestSuite.Endpoints.Contracts.Organizers;
using CrossTeamTestSuite.Endpoints.Extensions;
using CrossTeamTestSuite.Tests.Admin;
using CrossTeamTestSuite.Tests.Event;
using CrossTeamTestSuite.Tests.Examples;
using CrossTeamTestSuite.Tests.Organizer;
using CrossTeamTestSuite.Tests.User;
using CrossTeamTestSuite.TestsInfrastructure;

namespace CrossTeamTestSuite;

class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Wrong number of arguments provided");
        }

        var address = args[0];
        var adminEmail = args[1];
        var adminPassword = args[2];
        ApiClientSingleton.ConfigureClient(address);
        

        await new TestPipeline()
            .AddTest(new AdminLoginTest(adminEmail, adminPassword))
            .AddTest(new UserRegisterTest())
            .AddTest(new UserLoginTest())
            .AddTest(new OrganizerRegisterTest())
            .AddTest(new OrganizerDecideTest())
            .AddTest(new OrganizerLoginTest())
            .AddTest(new OrganizerViewTest())
            .AddTest(new UserViewTest())
            .AddTest(new EventCreateTest())
            .AddTest(new EventListTest())
            .GetExecutor()
            .ExecuteAsync();
    }
}

