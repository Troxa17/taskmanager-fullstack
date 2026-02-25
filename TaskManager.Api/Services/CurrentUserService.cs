using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http)
    {
        _http = http;
    }
    public int UserId
    {
        get
        {
            var user = _http.HttpContext?.User;
            var value = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Authenticated user id claim is missing ");
            if (!int.TryParse(value, out var id))
                throw new InvalidOperationException("Authenticated user id claim is invalid ");

            return id;
        }
    }
}