using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TicketManager.PaymentService.Api;
using TicketManager.PaymentService.Services.DataAccess;
using Xunit;

namespace TicketManager.IntegrationTests;

public class PaymentServiceApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string databaseConnectionString = Guid.NewGuid().ToString();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // comment for debug purposes
        builder.ConfigureLogging(logging => logging.ClearProviders());
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<PaymentDbContext>));
            services.Remove(descriptor!);

            services.RemoveAll<PaymentDbContext>();
            services.AddDbContext<PaymentDbContext>(
                opts => opts
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseInMemoryDatabase(databaseConnectionString)
            );
        });
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
