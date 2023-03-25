using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using TicketManager.Core.Api;
using Xunit;

namespace TicketManager.IntegrationTests;

public class TicketManagerApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase database = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "testDb",
            Username = "testUser",
            Password = "doesnt_matter",
        }).Build();
    
    public async Task InitializeAsync()
    {
        await database.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await database.DisposeAsync();
    }
}
