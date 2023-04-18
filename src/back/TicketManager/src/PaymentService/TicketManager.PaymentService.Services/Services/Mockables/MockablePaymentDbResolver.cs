using Microsoft.Extensions.DependencyInjection;
using TicketManager.PaymentService.Services.DataAccess;

namespace TicketManager.PaymentService.Services.Services.Mockables;

public class MockablePaymentDbResolver
{
    public virtual PaymentDbContext Resolve(IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    }
}
