namespace InvestmentAssistant.Api.Models.Requests;

/// <summary>
/// Żądanie rejestracji
/// </summary>
public record RegisterRequest(
    string Email,
    string Password,
    string FullName,
    string? PhoneNumber= null
);