using System.Collections.Generic;
using Flow.Tasks.Application;
using Flow.Tasks.Application.Tasks;
using Flow.Tasks.Api.DTOs;
using Flow.Tasks.Api.Mappers;
using Flow.Tasks.Api.Validation;

namespace Flow.Tasks.Api.Endpoints;

public static class TasksEndpoints
{
    public static IEndpointRouteBuilder MapTasks(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/tasks");

        g.MapGet("/", async (
                ITaskService svc,
                CancellationToken ct,
                int page = 1,
                int pageSize = 10,
                string? sortBy = null,
                bool desc = true,
                string? search = null,
                string? assignedTo = null,
                int? status = null) =>
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 || pageSize > 200 ? 20 : pageSize;

            var (items, total) = await svc.ListAsync(
                new TaskListQuery(page, pageSize, sortBy, desc, search, assignedTo, status), ct);

            var totalPages = (int)Math.Ceiling((double)total / pageSize);

            return Results.Ok(new
            {
                total,
                page,
                pageSize,
                totalPages,
                hasNext = page < totalPages,
                hasPrevious = page > 1,
                items = items.Select(i => i.ToResponse())
            });
        });


        // GET by id
        g.MapGet("/{id:int}", async (int id, ITaskService svc, CancellationToken ct) =>
        {
            var e = await svc.GetAsync(id, ct);
            return e is null ? Results.NotFound() : Results.Ok(e.ToResponse());
        });

        // POST create
        g.MapPost("/", async (CreateTaskRequest req, ITaskService svc, CancellationToken ct) =>
        {
            var e = await svc.CreateAsync(req.Title, req.Description, req.AssignedTo, req.Status, ct);
            return Results.Created($"/tasks/{e.Id}", e.ToResponse());
        })
        .AddEndpointFilter<ValidationFilter<CreateTaskRequest>>();

        // PATCH status (optimistic concurrency)
        g.MapPatch("/{id:int}", async (int id, UpdateTaskStatusRequest req, ITaskService svc, CancellationToken ct) =>
        {
            var (updated, conflict) = await svc.UpdateStatusAsync(id, req.Status, req.RowVersion, ct);
            if (conflict) return Results.Conflict(new { message = "RowVersion conflict. Reload and retry." });
            return updated is null ? Results.NotFound() : Results.Ok(updated.ToResponse());
        })
        .AddEndpointFilter<ValidationFilter<UpdateTaskStatusRequest>>();

        // DELETE soft
        g.MapDelete("/{id:int}", async (int id, ITaskService svc, CancellationToken ct) =>
        {
            var ok = await svc.DeleteAsync(id, ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
