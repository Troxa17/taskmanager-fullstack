using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public sealed class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Tasks
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);
    }
    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct)
    {
        if (id <= 0) throw new InvalidOperationException("Id must be highest then 0");

        return await _db.Tasks
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);
    }
    public async Task<TaskItem> CreateAsync(string title, CancellationToken ct)
    {
        title = NormalizeTitle(title);
        var entity = new TaskItem
        {
            Title = title,
            IsComleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };
        _db.Tasks.Add(entity);
        await _db.SaveChangesAsync(ct);

        return entity;
    }
    public async Task<bool> UpdateTitleAsync(int id, string title, CancellationToken ct)
    {
        if (id <= 0) throw new InvalidOperationException("Id must be highest then 0");
        title = NormalizeTitle(title);

        var entity = await _db.Tasks.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        entity.Title = title;

        await _db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> SetCompletedAsync(int id, bool IsComleted, CancellationToken ct)
    {
        if (id <= 0) throw new InvalidOperationException("Id must be highest then 0");
        var entity = await _db.Tasks.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        entity.IsComleted = IsComleted;

        await _db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        if (id <= 0) throw new InvalidOperationException("Id must be highest then 0");
        var entity = await _db.Tasks.SingleOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return false;

        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new InvalidOperationException("Title is required");

        title = title.Trim();

        if (title.Length > 200) throw new InvalidOperationException("Title must be under 200 characters");

        return title;
    }

}