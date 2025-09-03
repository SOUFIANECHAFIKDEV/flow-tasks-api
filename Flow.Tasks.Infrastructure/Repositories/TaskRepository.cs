using Flow.Tasks.Application;
using Flow.Tasks.Application.Abstractions;
using Flow.Tasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flow.Tasks.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly Data.AppDbContext _db;
    public TaskRepository(Data.AppDbContext db) => _db = db;

    //public async Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(TaskListQuery q, CancellationToken ct)
    //{
    //    var query = _db.Tasks.AsQueryable();

    //    if (!string.IsNullOrWhiteSpace(q.Search))
    //        query = query.Where(t => t.Title.Contains(q.Search!) || (t.Description != null && t.Description.Contains(q.Search!)));

    //    query = (q.SortBy?.ToLowerInvariant()) switch
    //    {
    //        "title" => q.Desc ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
    //        "status" => q.Desc ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
    //        _ => q.Desc ? query.OrderByDescending(t => t.CreatedAtUtc) : query.OrderBy(t => t.CreatedAtUtc)
    //    };

    //    var total = await query.CountAsync(ct);
    //    var items = await query.AsNoTracking()
    //                           .Skip((q.Page - 1) * q.PageSize)
    //                           .Take(q.PageSize)
    //                           .ToListAsync(ct);

    //    return (items, total);
    //}

    public async Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(TaskListQuery q, CancellationToken ct)
    {
        var query = _db.Tasks.Where(t => !t.IsDeleted).AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var like = $"%{q.Search}%";
            query = query.Where(t =>
                EF.Functions.Like(t.Title, like) ||
                (t.Description != null && EF.Functions.Like(t.Description, like)));
        }

        // AssignedTo
        if (!string.IsNullOrWhiteSpace(q.AssignedTo))
        {
            var likeAss = $"%{q.AssignedTo}%";
            query = query.Where(t => t.AssignedTo != null && EF.Functions.Like(t.AssignedTo, likeAss));
        }

        // Status
        if (q.Status is not null)
        {
            query = query.Where(t => (int)t.Status == q.Status.Value);
        }

        // Tri
        var key = (q.SortBy ?? "createdAtUtc").ToLowerInvariant();
        var ordered = key switch
        {
            "title" => q.Desc ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "assignedto" => q.Desc ? query.OrderByDescending(t => t.AssignedTo) : query.OrderBy(t => t.AssignedTo),
            "status" => q.Desc ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            "updatedatutc" => q.Desc ? query.OrderByDescending(t => t.UpdatedAtUtc) : query.OrderBy(t => t.UpdatedAtUtc),
            _ => q.Desc ? query.OrderByDescending(t => t.CreatedAtUtc) : query.OrderBy(t => t.CreatedAtUtc)
        };

        var total = await ordered.CountAsync(ct);

        var items = await ordered
            .AsNoTracking()
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }


    public Task<TaskItem?> GetAsync(int id, CancellationToken ct)
        => _db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<TaskItem?> GetAsyncAsTracking(int id, CancellationToken ct)
    => _db.Tasks.AsTracking().FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task AddAsync(TaskItem entity, CancellationToken ct)
    {
        _db.Tasks.Add(entity);
        return Task.CompletedTask;
    }

    public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

    public async Task SoftDeleteAsync(int id, CancellationToken ct)
    {
        var tracked = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (tracked is not null)
        {
            tracked.IsDeleted = true;
            tracked.UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
