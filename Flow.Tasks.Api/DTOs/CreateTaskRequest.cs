using Flow.Tasks.Api.Domain;
using TaskStatus = Flow.Tasks.Api.Domain.TaskStatus;

namespace Flow.Tasks.Api.DTOs;

public sealed class CreateTaskRequest
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? AssignedTo { get; set; }
    public TaskStatus? Status { get; set; } // optionnel, par défaut Todo
}