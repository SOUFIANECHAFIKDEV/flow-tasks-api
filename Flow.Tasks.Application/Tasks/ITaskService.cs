using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Application.Tasks;

public interface ITaskService
{
    Task<(IReadOnlyList<TaskItem> Items, int Total)> ListAsync(TaskListQuery query, CancellationToken ct);
    Task<TaskItem?> GetAsync(int id, CancellationToken ct);
    Task<TaskItem> CreateAsync(string title, string? description, string? assignedTo, int Status, CancellationToken ct);
    Task<(TaskItem? Updated, bool Conflict)> UpdateStatusAsync(int id, int status, string rowVersion, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
