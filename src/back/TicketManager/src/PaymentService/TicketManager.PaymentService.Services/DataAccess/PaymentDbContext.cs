using Microsoft.EntityFrameworkCore;
using TicketManager.PaymentService.Domain.Payments;

namespace TicketManager.PaymentService.Services.DataAccess;

public class PaymentDbContext : DbContext
{
    public virtual DbSet<Payment> Payments => Set<Payment>();
    
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    { }
    
#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.EnableSensitiveDataLogging();
    }
#endif
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurePayments(modelBuilder);
    }

    private void ConfigurePayments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(cfg =>
        {
            cfg.HasKey(e => e.Id);
        });
    }
}
