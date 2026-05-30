using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Infrastructure.Database;

/// <summary>
/// Kontekst bazy danych dla aplikacji Investment Assistant.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Tabela Users - reprezentuje użytkowników aplikacji
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///  Tabela Akcji - reprezentuje dane o akcjach giełdowych
    /// </summary>
    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);

            entity.Property(u => u.FullName).IsRequired().HasMaxLength(255);

            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);

            entity.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired(false);

            entity.Property(u => u.IsActive).HasDefaultValue(true);

            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            entity.ToTable("users");
        });

         modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.Symbol).IsUnique();
            
            entity.Property(s => s.Symbol)
                .IsRequired()
                .HasMaxLength(10);
            
            entity.Property(s => s.CurrentPrice)
                .HasPrecision(18, 2);
            
            entity.Property(s => s.OpenPrice)
                .HasPrecision(18, 2);
            
            entity.Property(s => s.HighPrice)
                .HasPrecision(18, 2);
            
            entity.Property(s => s.LowPrice)
                .HasPrecision(18, 2);
            
            entity.Property(s => s.PreviousClosePrice)
                .HasPrecision(18, 2);
            
            entity.Property(s => s.LastUpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
            
            entity.ToTable("stocks");
        });
    }
}