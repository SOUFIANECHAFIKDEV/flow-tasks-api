namespace Flow.Tasks.Api.DTOs;

public sealed record TaskResponse(
    int Id,
    string Title,
    string? Description,
    string? AssignedTo,
    DateTime? createdAtUtc,
    int Status,
    string RowVersion);

public sealed record CreateTaskRequest(
    string Title,
    string? Description,
    string? AssignedTo,
    int Status);

public sealed record UpdateTaskStatusRequest(
    int Status,
    string RowVersion);
