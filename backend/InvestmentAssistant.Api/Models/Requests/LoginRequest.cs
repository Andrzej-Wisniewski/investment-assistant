namespace InvestmentAssistant.Api.Models.Responses;

/// <summary>
/// Reprezentuje żądanie logowania.
/// </summary>
public record LoginRequest(string Email, string Password, string FullName, string PhoneNumber);
