using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Models.Requests;
using InvestmentAssistant.Api.Models.Responses;
using InvestmentAssistant.Api.Repositories;

namespace InvestmentAssistant.Api.Services
{
    /// <summary>
    /// Implementacja serwisu uwierzytelniania.
    /// Zawiera logikę biznesową: sprawdzenie haseł, generowanie tokenów.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        // Wstrzykiwanie zależności przez konstruktor
        // IUserRepository zostaje wstrzyknięty przez framework
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new UnauthorizedAccessException("Email i hasło są wymagane.");
            }

            // Pobierz użytkownika z bazy danych na podstawie adresu email
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Niepoprawny email lub hasło.");
            }

            // Generowanie JWT
            var token = "placeholder-token";

            return new LoginResponse(
                AccessToken: token,
                ExpiresIn: 3600);
        }

        public async Task<UserResponse> RegisterAsync(LoginRequest request)
        {
            // Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Email i hasło są wymagane.");
            }

            // Sprawdzenie, czy użytkownik już istnieje
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Użytkownik z tym adresem email już istnieje.");
            }


            // Tworzenie nowego użytkownika
            var user = new User
            {
                Email = request.Email,
                FullName = request.Email.Split('@')[0],
                PasswordHash = HashPassword(request.Password),
            };

            var createdUser = await _userRepository.CreateAsync(user);


            return new UserResponse(
                Id: createdUser.Id,
                Email: createdUser.Email,
                FullName: createdUser.FullName,
                CreatedAt: createdUser.CreatedAt
            );
        }

        /// <summary>
        /// Hashuje hasło za pomocą BCrypt.
        /// </summary>
        private string HashPassword(string password)
        {
            // TODO: Implementacja hashowania hasła
            return password;
        }

        /// <summary>
        /// Sprawdza, czy podane hasło jest poprawne w porównaniu do przechowywanego hash'a.    
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            // TODO: Implementacja BCrypt
            return password == hash;
        }
    }
}