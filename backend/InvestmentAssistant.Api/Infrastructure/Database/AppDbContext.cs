using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Infrastructure.Database;

/// <summary>
/// Kontekst bazy danych dla aplikacji Investment Assistant.
/// Kazda klasa Dbset reprezentuje tabelę w bazie danych.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Tabela Users - reprezentuje użytkowników aplikacji.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Konfiguracja modelu danych - definiuje relacje, klucze itp.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Konfiguracja tabeli Users
        modelBuilder.Entity<User>(entity =>
        {
            // Klucz główny
            entity.HasKey(u => u.Id);

            // Email musi być unikalny
            entity.HasIndex(u => u.Email).IsUnique();

            // Email jest wymagany i ma maksymalną długość
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);

            // FullName jest obowiązkowy
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(255);

            // PasswordHash jest obowiązkowy
            entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);

            // Numer telefonu jest opcjonalny
            entity.Property(u => u.PhoneNumber).HasMaxLength(20);

            // isActive ma wartość domyślną (true)
            entity.Property(u => u.IsActive).HasDefaultValue(true);

            // CreatedAt ma wartość domyślną (aktualny czas)
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

            // Nazwa tabeli w bazie danych
            entity.ToTable("users");
        });
    }
}