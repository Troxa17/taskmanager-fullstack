using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs.Requests;
using TaskManager.Api.DTOs.Response;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IJwtTokenService _jwt;
    public AuthController(IAuthService auth, IJwtTokenService jwt)
    {
        _auth = auth;
        _jwt = jwt;
    }
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var user = await _auth.RegisterAsync(request.Email, request.Password, ct);
        var token = _jwt.CreateToken(user);

        return Ok(new AuthResponse { AccessToken = token });
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var user = await _auth.ValidateUserAsync(request.Email, request.Password, ct);
        if (user is null) return Unauthorized();

        var token = _jwt.CreateToken(user);
        return Ok(new AuthResponse { AccessToken = token });
    }
}