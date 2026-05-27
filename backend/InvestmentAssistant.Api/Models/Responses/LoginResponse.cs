namespace InvestmentAssistant.Api.Models.Requests;

    /// <summary>
    /// Odpowiedź na próbę logowania.
    /// Zawiera token JWT, którym klient będzie się posługiwać w przyszłych żądaniach.
    /// </summary>
    public record LoginResponse(string AccessToken, string TokenType = "Bearer", int ExpiresIn = 3600);