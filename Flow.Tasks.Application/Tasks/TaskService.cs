using Flow.Tasks.Application.Abstractions;
using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Application.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;
    public TaskService(ITaskRepository repo) => _repo = repo;

    public Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(TaskListQuery query, CancellationToken ct)
        => _repo.ListAsync(query, ct);

    public Task<TaskItem?> GetAsync(int id, CancellationToken ct)
        => _repo.GetAsync(id, ct);

    public async Task<TaskItem> CreateAsync(string title, string? description, int? AssignedUserId, int Status, CancellationToken ct)
    {
        var e = new TaskItem
        {
            Title = title,
            Description = description,
            AssignedUserId = AssignedUserId,
            Status = (Domain.Enums.TaskStatus) Status,
            CreatedAtUtc = DateTime.Now
        };
        await _repo.AddAsync(e, ct);
        await _repo.SaveAsync(ct);
        return e;
    }


    public async Task<(TaskItem? Updated, bool Conflict)> UpdateStatusAsync(int id, int status, string rowVersion, CancellationToken ct)
    {
        var entity = await _repo.GetAsyncAsTracking(id, ct);
        if (entity is null || entity.IsDeleted) return (null, false);

        byte[] incoming = ParseRowVersion(rowVersion);
        if (!entity.RowVersion.SequenceEqual(incoming))
            return (null, true);

        entity.Status = (Domain.Enums.TaskStatus)status;
        entity.UpdatedAtUtc = DateTime.Now;
        await _repo.SaveAsync(ct);
        return (entity, false);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var existing = await _repo.GetAsync(id, ct);
        if (existing is null || existing.IsDeleted) return false;
        await _repo.SoftDeleteAsync(id, ct);
        await _repo.SaveAsync(ct);
        return true;
    }

    private static byte[] ParseRowVersion(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("RowVersion manquante.");
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return Convert.FromHexString(s[2..]);
        return Convert.FromBase64String(s);
    }
}
