using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;
using TicketManager.Core.Domain.Users;

namespace TicketManager.Core.Services.DataAccess;

public class CoreDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();

    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureAccounts(modelBuilder);
    }

    private void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Email).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);

            // cfg.IsOptimisticConcurrent();
        });
    }
    
    private void ConfigureAccounts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Email).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.PasswordHash).HasMaxLength(StringLengths.ShortString);
            
            // cfg.IsOptimisticConcurrent();
        });
    }
}

public static class ModelBuilderExtensions
{
    public static void IsOptimisticConcurrent<TEntity>(this EntityTypeBuilder<TEntity> cfg)
        where TEntity : class, IOptimisticConcurrent
    {
        cfg.Property<byte[]>("RowVersion")
            .HasColumnName("RowVersion")
            .IsRowVersion()
            .IsRequired();
        cfg.Property(e => e.DateModified).IsConcurrencyToken();
    }
}
