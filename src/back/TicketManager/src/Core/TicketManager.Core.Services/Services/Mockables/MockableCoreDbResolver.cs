using Microsoft.Extensions.DependencyInjection;
using TicketManager.Core.Services.DataAccess;

namespace TicketManager.Core.Services.Services.Mockables;

public class MockableCoreDbResolver
{
    public virtual CoreDbContext Resolve(IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<CoreDbContext>();
    }
}
