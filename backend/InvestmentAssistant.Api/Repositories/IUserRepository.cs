using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Repositories;

/// <summary>
/// Abstrakcja dostępu do danych użytkowników.
/// Separuje logikę biznesową od sposobu przechowywania danych.
/// </summary>
public interface IUserRepository
{    /// <summary>
     /// Pobiera użytkownika na podstawie jego identyfikatora.
     /// </summary>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Pobiera użytkownika na podstawie jego adresu e-mail.
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Dodaje nowego użytkownika do repozytorium.
    /// </summary>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Zapisuje zmiany w użytkowniku.
    /// </summary>
    Task<User> UpdateAsync(User user);
}