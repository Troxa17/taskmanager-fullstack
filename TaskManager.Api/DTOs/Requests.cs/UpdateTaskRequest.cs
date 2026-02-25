using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs.Requests;

public sealed class UpdateTaskRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
