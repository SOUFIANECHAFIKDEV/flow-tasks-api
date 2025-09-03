using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Application.Abstractions;

public interface ITaskRepository
{
    Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(TaskListQuery query, CancellationToken ct);
    Task<TaskItem?> GetAsync(int id, CancellationToken ct);
    Task<TaskItem?> GetAsyncAsTracking(int id, CancellationToken ct);
    Task AddAsync(TaskItem entity, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
    Task SoftDeleteAsync(int id, CancellationToken ct);
}
