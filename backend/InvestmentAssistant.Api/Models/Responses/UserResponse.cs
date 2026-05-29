namespace InvestmentAssistant.Api.Models.Responses;

/// <summary>
/// Reprezentacja użytkownika w odpowiedziach API
/// </summary>
public record UserResponse(Guid Id, string Email, string FullName, string? PhoneNumber, DateTime CreatedAt);
