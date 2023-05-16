using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TicketManager.Core.Api;
using TicketManager.Core.Services.DataAccess;
using TicketManager.Core.Services.Services.HttpClients;
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

    private readonly HttpClient paymentClient;

    public TicketManagerApp(HttpClient paymentClient)
    {
        this.paymentClient = paymentClient;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // comment for debug purposes
        builder.ConfigureLogging(logging => logging.ClearProviders());
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<CoreDbContext>));
            services.Remove(descriptor!);

            services.RemoveAll<CoreDbContext>();
            services.AddDbContext<CoreDbContext>(
                opts => opts
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseNpgsql(database.ConnectionString)
            );

            services.AddMassTransitTestHarness();

            // big hack, but for our needs it's gonna work
            services.RemoveAll<PaymentClient>();
            services.AddSingleton(paymentClient);
            services.AddSingleton<PaymentClient>();
        });
    }
    
    public async Task InitializeAsync()
    {
        await database.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await database.DisposeAsync();
    }
}
