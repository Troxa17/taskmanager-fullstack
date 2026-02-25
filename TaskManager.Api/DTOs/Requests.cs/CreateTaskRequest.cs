using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs.Requests;

public sealed class CreateTaskRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
}