using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs.Requests;

public sealed class UpdateTitleRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
}