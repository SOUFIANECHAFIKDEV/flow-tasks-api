namespace Flow.Tasks.Api.DTOs;

public sealed record UserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email);