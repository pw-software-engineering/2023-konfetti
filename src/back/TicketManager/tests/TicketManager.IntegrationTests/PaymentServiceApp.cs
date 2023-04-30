using Microsoft.AspNetCore.Mvc.Testing;
using TicketManager.PaymentService.Api;
using Xunit;

namespace TicketManager.IntegrationTests;

public class PaymentServiceApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
