using Flow.Tasks.Api.Domain;

namespace Flow.Tasks.Api.DTOs;

public sealed class TaskResponse : BaseEntity
{
    public TaskResponse()
    {
    }

    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Domain.TaskStatus Status { get; set; }
    public string? AssignedTo { get; set; }
    public string RowVersion { get; set; } = default!;
}