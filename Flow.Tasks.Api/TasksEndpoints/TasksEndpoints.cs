using AutoMapper;
using Flow.Tasks.Api.Data;
using Flow.Tasks.Api.Domain;
using Flow.Tasks.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Flow.Tasks.Api.Endpoints;

public static class TasksEndpoints
{
    public static IEndpointRouteBuilder MapTasks(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks").WithTags("Tasks");

        // POST /tasks
        group.MapPost("/", async (CreateTaskRequest req, AppDbContext db, IMapper mapper) =>
        {
            var entity = mapper.Map<TaskEntity>(req);
            db.Tasks.Add(entity);
            await db.SaveChangesAsync();
            return Results.Created($"/tasks/{entity.Id}", mapper.Map<TaskResponse>(entity));
        })
        .Produces<TaskResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // GET /tasks
        group.MapGet("/", async (AppDbContext db, IMapper mapper) =>
        {
            var list = await db.Tasks
                .Where(t => !t.IsDeleted) // (ignore soft-deleted)
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync();

            return Results.Ok(mapper.Map<List<TaskResponse>>(list));
        })
        .Produces<List<TaskResponse>>();

        // PATCH /tasks/{id} = update status
        group.MapPatch("/{id:int}", async (int id, UpdateTaskStatusRequest req, AppDbContext db, IMapper mapper) =>
        {
            var entity = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (entity is null) return Results.NotFound();

            var incoming = ParseRowVersion(req.RowVersion);
            if (!entity.RowVersion.SequenceEqual(incoming))
                return Results.Conflict(new { message = "RowVersion conflict. Reload and retry." });

            entity.Status = req.Status;
            await db.SaveChangesAsync();
            return Results.Ok(mapper.Map<TaskResponse>(entity));
        })
        .Produces<TaskResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        //DELETE soft: /tasks/{id}
        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (entity is null) return Results.NotFound();

            entity.IsDeleted = true;
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    static byte[] ParseRowVersion(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException("RowVersion manquante.");

        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return Convert.FromHexString(s.AsSpan(2)); // .NET 5+

        return Convert.FromBase64String(s);
    }
}
