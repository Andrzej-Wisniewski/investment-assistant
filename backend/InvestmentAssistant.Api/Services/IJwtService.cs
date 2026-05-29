using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Services;

///<summary>
/// Serwis do obsługi tokenów JWT
/// </summary>
public interface IJwtService
{
    ///<summary>
    /// Generuje token JWT dla podanego użytkownika
    ///</summary>
    String GenerateToken(User user);

    ///<summary>
    /// Waliduje token i zwraca ID użytkownika z tokenu
    /// </summary>
    Guid? ValidateToken(string token);
}