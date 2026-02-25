using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(string email, string password, CancellationToken ct);
    Task<User?> ValidateUserAsync(string email, string password, CancellationToken ct);
}