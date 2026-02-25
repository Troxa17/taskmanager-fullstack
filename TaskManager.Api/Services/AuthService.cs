using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<User> RegisterAsync(string email, string password, CancellationToken ct)
    {
        email = email.Trim().ToLowerInvariant();

        var exists = await _db.Users.AnyAsync(x => x.Email == email, ct);
        if (exists)
            throw new InvalidOperationException("User with this mail already exist");
        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreateAtUtsc = DateTime.UtcNow
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return user;
    }
    public async Task<User?> ValidateUserAsync(string email, string password, CancellationToken ct)
    {
        email = email.Trim().ToLowerInvariant();

        var user = await _db.Users.SingleOrDefaultAsync(x => x.Email == email, ct);
        if (user is null) return null;
        var ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!ok) return null;

        return user;
    }
}