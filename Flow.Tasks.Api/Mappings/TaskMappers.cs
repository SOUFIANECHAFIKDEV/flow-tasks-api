using Flow.Tasks.Api.DTOs;
using Flow.Tasks.Domain.Entities;

namespace Flow.Tasks.Api.Mappers;

public static class TaskMappers
{
    public static TaskResponse ToResponse(this TaskItem e) =>
        new(e.Id, e.Title, e.Description, e.AssignedTo, e.CreatedAtUtc, (int)e.Status, Convert.ToBase64String(e.RowVersion));
}
