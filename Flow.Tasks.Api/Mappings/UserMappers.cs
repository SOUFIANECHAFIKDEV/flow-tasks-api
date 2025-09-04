using Flow.Tasks.Api.DTOs;
using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Api.Mappers;

public static class UserMappers
{
    public static UserResponse ToResponse(this UserEntity e) =>
        new(e.Id, e.FirstName, e.LastName, e.Email);
}
