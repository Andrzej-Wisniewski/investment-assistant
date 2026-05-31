using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Infrastructure.Database;

/// <summary>
/// Kontekst bazy danych dla aplikacji Investment Assistant.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockHistory> StockHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // === USERS ===
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.FullName)
            .HasMaxLength(255)
            .IsRequired();

        // === STOCKS ===
        modelBuilder.Entity<Stock>()
            .HasIndex(s => s.Symbol)
            .IsUnique();

        modelBuilder.Entity<Stock>()
            .Property(s => s.Symbol)
            .HasMaxLength(10)
            .IsRequired();

        // === STOCK HISTORY ===
        modelBuilder.Entity<StockHistory>()
            .HasOne(h => h.Stock)
            .WithMany(s => s.History)
            .HasForeignKey(h => h.StockId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StockHistory>()
            .HasIndex(h => new { h.StockId, h.Date })
            .IsUnique();

        modelBuilder.Entity<StockHistory>()
            .Property(h => h.Date)
            .HasColumnType("date");
    }
}