using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using TicketManager.Core.Domain.Accounts;
using TicketManager.Core.Domain.Common;
using TicketManager.Core.Domain.Events;
using TicketManager.Core.Domain.Organizer;
using TicketManager.Core.Domain.Users;
using TicketManager.Core.Services.Services.PasswordManagers;

namespace TicketManager.Core.Services.DataAccess;

public class CoreDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public virtual DbSet<Organizer> Organizers => Set<Organizer>();
    public virtual DbSet<Account> Accounts => Set<Account>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Sector> Sectors => Set<Sector>();

    public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
    { }
    
#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.EnableSensitiveDataLogging();
    }
#endif

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureOrganizers(modelBuilder);
        ConfigureAccounts(modelBuilder);
        ConfigureEvents(modelBuilder);
        ConfigureSectors(modelBuilder);
    }

    private void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Email).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.FirstName).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.LastName).HasMaxLength(StringLengths.ShortString);

            cfg.IsOptimisticConcurrent();
        });
    }
    
    private void ConfigureOrganizers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organizer>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Email).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.CompanyName).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.TaxId).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.Address).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.DisplayName).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.PhoneNumber).HasMaxLength(StringLengths.ShortString);

            cfg.IsOptimisticConcurrent();
        });
    }
    
    private void ConfigureAccounts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Email).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.PasswordHash).HasMaxLength(StringLengths.MediumString);
            cfg.Property(e => e.Role).HasMaxLength(StringLengths.LittleString);

            cfg.IsOptimisticConcurrent();
        });
    }
    
    private void ConfigureEvents(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(cfg =>
        {
            cfg.HasKey(e => e.Id);
            cfg.Property(e => e.Name).HasMaxLength(StringLengths.ShortString);
            cfg.Property(e => e.Description).HasMaxLength(StringLengths.VeryLongString);
            cfg.Property(e => e.Location).HasMaxLength(StringLengths.MediumString);

            cfg.IsOptimisticConcurrent();
        });
    }
    
    private void ConfigureSectors(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sector>(cfg =>
        {
            cfg.HasKey(s => s.Id);
            cfg.Property(s => s.Name).HasMaxLength(StringLengths.ShortString);
            cfg.OwnsMany(s => s.SeatReservations, scfg =>
            {
                scfg.HasKey(s => s.Id);
                scfg.WithOwner().HasForeignKey(s => s.SectorId);
                scfg.Property(s => s.CreationDate);
            });
            cfg.OwnsMany(s => s.TakenSeats);
            
            cfg.IsOptimisticConcurrent();
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
            .IsRequired()
            .HasDefaultValue(Array.Empty<byte>());
        cfg.Property(e => e.DateModified).IsConcurrencyToken();
    }
}
