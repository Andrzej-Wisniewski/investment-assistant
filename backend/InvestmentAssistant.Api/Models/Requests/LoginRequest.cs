namespace InvestmentAssistant.Api.Models.Requests;

/// <summary>
/// Reprezentuje żądanie logowania.
/// </summary>
public record LoginRequest(string Email, string Password);
