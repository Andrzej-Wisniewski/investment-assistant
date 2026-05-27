namespace InvestmentAssistant.Api.Models.Responses;

/// <summary>
/// Reprezentacja użytkownika w odpowiedziach API.
/// Nie zawiera wrażliwych danych (jak hasło).
/// </summary>
public record UserResponse(Guid Id, string Email, string FullName, DateTime CreatedAt);
