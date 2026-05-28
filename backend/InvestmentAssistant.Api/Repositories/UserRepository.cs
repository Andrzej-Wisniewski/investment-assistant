using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Models.Entities;
using InvestmentAssistant.Api.Infrastructure.Database;

namespace InvestmentAssistant.Api.Repositories;

/// <summary>
/// Implementacja dostępu do danych użytkowników (PostgreSQL + EF Core).
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        user.UpdatedAt = DateTime.UtcNow;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }
}