namespace TaskManager.Api.Models;

public sealed class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComleted { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}
