using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Models.Requests;
using InvestmentAssistant.Api.Models.Responses;
using InvestmentAssistant.Api.Repositories;
using InvestmentAssistant.Api.Infrastructure.Cache;
using BCrypt.Net;
namespace InvestmentAssistant.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, ICacheService cacheService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new UnauthorizedAccessException("Email i hasło są wymagane.");
            }

            var cacheKey = $"user:{request.Email}";
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);

            User? user;

            if(cachedUser is not null)
            {
                user = cachedUser;
            }
            else
            {
                user = await _userRepository.GetByEmailAsync(request.Email);

                if (user is not null)
                {
                    await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(5));
                }
            }

            if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Niepoprawny email lub hasło.");
            }

            var token = _jwtService.GenerateToken(user);

            return new LoginResponse(
                AccessToken: token,
                ExpiresIn: 3600);
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new ArgumentException("Email, hasło i imię są wymagane.");
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Użytkownik z tym adresem email już istnieje.");
            }

            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.Password),
            };

            var createdUser = await _userRepository.CreateAsync(user);
            await _cacheService.RemoveAsync($"user:{request.Email}");

            return new UserResponse(
                Id: createdUser.Id,
                Email: createdUser.Email,
                FullName: createdUser.FullName,
                PhoneNumber: createdUser.PhoneNumber,
                CreatedAt: createdUser.CreatedAt
            );
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch 
            {
                return false;
            }
        }
    }
}