using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface IJwtTokenService
{
    string CreateToken(User user);
}