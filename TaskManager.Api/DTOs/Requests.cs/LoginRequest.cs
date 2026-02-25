using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs.Requests;

public sealed class LoginRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}