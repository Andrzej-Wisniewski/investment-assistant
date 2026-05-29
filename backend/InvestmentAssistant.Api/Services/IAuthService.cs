using InvestmentAssistant.Api.Models.Requests;
using InvestmentAssistant.Api.Models.Responses;

namespace InvestmentAssistant.Api.Services;

/// <summary>
/// Serwis obsługujący uwierzytelnianie i autoryzację użytkowników.
/// </summary>
public interface IAuthService
{    /// <summary>
     /// Loguje użytkownika na podstawie emailu i hasła.
     /// Zwraca JWT token, jeśli dane są poprawne.
     /// </summary>
    Task<LoginResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Rejestruje nowego użytkownika na podstawie podanych danych.
    /// </summary>
    Task<UserResponse> RegisterAsync(RegisterRequest request);
}
