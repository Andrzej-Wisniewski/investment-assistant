using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

///<summary>
/// Implementacja serwisu JWT
/// </summary>
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly byte[] _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT:SecretKey nie jest skonfigurowany"));
        _issuer = _configuration["Jwt:Issuer"] ?? "InvestmentAssistant";
        _audience = _configuration["Jwt:Audience"] ?? "InvestmentAssistantClient";
        _expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
    }

    ///<summary>
    /// Generuje token JWT dla podanego użytkownika
    ///</summary>
    public string GenerateToken(User user)
    {
        // Klucz do podpisu tokenu
        var key = new SymmetricSecurityKey(_secretKey);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Informacja o użytkowniku, która będzie zawarta w tokenie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
        };

        // Konfiguracja tokenu
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = credentials
        };

        // Generujemy token
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        // Serializujemy na string
        return handler.WriteToken(token);
    }

    ///<summary>
    /// Waliduje token i zwraca ID użytkownika z tokenu
    /// </summary>
    public Guid? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        try
        {
            var key = new SymmetricSecurityKey(_secretKey);
            var handler = new JwtSecurityTokenHandler();

            // Walidacja tokenu
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
            }, out SecurityToken validatedToken);

            // Pobieramy ID użytkownika z tokenu
            var userIdClaim = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }
        catch
        {
            // Token jest nie ważny
            return null;
        }
    }
}