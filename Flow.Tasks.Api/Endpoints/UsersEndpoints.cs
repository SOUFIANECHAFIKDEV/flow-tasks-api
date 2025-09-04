using Flow.Tasks.Api.DTOs;
using Flow.Tasks.Api.Mappers;
using Flow.Tasks.Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Flow.Tasks.Api.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/users");

        g.MapGet("/", async (
                [FromServices] IUserService svc,
                CancellationToken ct) =>
        {
            var users = await svc.ListAsync(ct);

            //var result = users.Select(u => new UserResponse(
            //    u.Id,
            //    u.FirstName,
            //    u.LastName,
            //    u.Email
            //)).ToList();

            return Results.Ok(users.Select(i => i.ToResponse()));
        });

        return app;
    }
}
