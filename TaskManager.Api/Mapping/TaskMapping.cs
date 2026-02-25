using TaskManager.Api.DTOs.Response;
using TaskManager.Api.Models;

namespace TaskManager.Api.Mapping;

public static class TaskMapping
{
    public static TaskResponse ToResponse(this TaskItem entity)
    {
        return new TaskResponse
        {
            Id = entity.Id,
            Title = entity.Title,
            IsCompleted = entity.IsComleted,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }
}