namespace Flow.Tasks.Api.DTOs;

public sealed record TaskResponse(
    int Id,
    string Title,
    string? Description,
    int? AssignedUserId,
    DateTime? createdAtUtc,
    int Status,
    string RowVersion,
    UserResponse? User);

public sealed record CreateTaskRequest(
    string Title,
    string? Description,
    int? AssignedUserId,
    int Status);

public sealed record UpdateTaskStatusRequest(
    int Status,
    string RowVersion);
