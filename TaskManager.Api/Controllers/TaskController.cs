using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;
using TaskManager.Api.DTOs.Requests;
using TaskManager.Api.DTOs.Response;
using TaskManager.Api.Mapping;
using TaskManager.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/task")]
[Authorize]
public sealed class TaskController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ICurrentUserService _current;
    public TaskController(AppDbContext db, ICurrentUserService current)
    {
        _db = db;
        _current = current;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetMyTask(CancellationToken ct)
    {
        var userId = _current.UserId;
        var items = await _db.Tasks
            .Where(x => x.OwnerId == userId)
            .OrderByDescending(x => x.Id)
            .Select(x => new TaskResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsCompleted = x.IsComleted,
                CreatedAtUtc = x.CreatedAtUtc
            }).ToListAsync(ct);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest request, CancellationToken ct)
    {
        var userId = _current.UserId;
        var entity = new TaskItem
        {
            Title = request.Title.Trim(),
            IsComleted = false,
            CreatedAtUtc = DateTime.UtcNow,
            OwnerId = userId
        };
        _db.Tasks.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Ok(new TaskResponse
        {
            Id = entity.Id,
            Title = entity.Title,
            IsCompleted = entity.IsComleted,
            CreatedAtUtc = entity.CreatedAtUtc
        });
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTaskRequest request, CancellationToken ct)
    {
        var userId = _current.UserId;
        var entity = await _db.Tasks
            .SingleOrDefaultAsync(x => x.Id == id && x.OwnerId == userId, ct);

        if (entity is null) return NotFound();
        entity.Title = request.Title.Trim();
        entity.IsComleted = request.IsCompleted;

        await _db.SaveChangesAsync(ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var userId = _current.UserId;
        var entity = await _db.Tasks
            .SingleOrDefaultAsync(x => x.Id == id && x.OwnerId == userId, ct);

        if (entity is null) return NotFound();

        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }
}