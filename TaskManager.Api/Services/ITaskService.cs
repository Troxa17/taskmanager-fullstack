using Microsoft.AspNetCore.Razor.TagHelpers;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ct);
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct);
    Task<TaskItem> CreateAsync(string title, CancellationToken ct);
    Task<bool> UpdateTitleAsync(int id, string title, CancellationToken ct);
    Task<bool> SetCompletedAsync(int id, bool IsCompleted, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}