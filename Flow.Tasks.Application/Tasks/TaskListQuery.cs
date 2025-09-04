namespace Flow.Tasks.Application;

public record TaskListQuery(
    int Page,
    int PageSize,
    string? SortBy,
    bool Desc,
    string? Search,
    int? AssignedUserId,
    int? Status
);

