namespace Flow.Tasks.Application;

//public sealed record TaskListQuery(
//    int Page = 1,
//    int PageSize = 20,
//    string? SortBy = "createdAt",
//    bool Desc = true,
//    string? Search = null);

public record TaskListQuery(
    int Page,
    int PageSize,
    string? SortBy,
    bool Desc,
    string? Search,
    string? AssignedTo,
    int? Status
);

