using InvestmentAssistant.Api.Models.Entities;

namespace InvestmentAssistant.Api.Repositories;

/// <summary>
/// Implementacja dostępu do danych użytkowników.
/// </summary>
public class UserRepository : IUserRepository
{
    public Task<User?> GetUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public Task<User?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}