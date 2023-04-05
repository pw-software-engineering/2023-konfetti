using Microsoft.EntityFrameworkCore;

namespace TicketManager.PaymentService.Services.DataAccess;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    { }
    
#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.EnableSensitiveDataLogging();
    }
#endif
}
