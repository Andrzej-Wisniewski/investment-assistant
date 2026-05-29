using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Models.Requests;
using InvestmentAssistant.Api.Models.Responses;
using InvestmentAssistant.Api.Repositories;
namespace InvestmentAssistant.Api.Services
{
    /// <summary>
    /// Implementacja serwisu uwierzytelniania.
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

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            // Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new ArgumentException("Email, hasło i imię są wymagane.");
            }

            // Sprawdzenie, czy użytkownik już istnieje
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Użytkownik z tym adresem email już istnieje.");
            }


            // Stwórz nowego użytkownika
            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.Password),
            };

            var createdUser = await _userRepository.CreateAsync(user);


            return new UserResponse(
                Id: createdUser.Id,
                Email: createdUser.Email,
                FullName: createdUser.FullName,
                PhoneNumber: createdUser.PhoneNumber,
                CreatedAt: createdUser.CreatedAt
            );
        }

        /// <summary>
        /// Hashuje hasło za pomocą BCrypt
        /// </summary>
        private string HashPassword(string password)
        {
            // BCrypt.HashPassword haszy i automatycznie dodaje sól
            // Wynik to 60-znakowy hash, którym można bezpiecznie przechowywać
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Sprawdza, czy podane hasło jest poprawne w porównaniu do przechowywanego hash'a.    
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch 
            {
                // Jeśli hash jest uszkodzony lub nieprawidłowy, zwóć false zamiast rzucać wyjątek
                return false;
            }
        }
    }
}