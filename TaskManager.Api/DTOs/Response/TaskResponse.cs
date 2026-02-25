namespace TaskManager.Api.DTOs.Response;

public sealed class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}