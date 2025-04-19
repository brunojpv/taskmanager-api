using System.Security.Claims;
using TaskManager.Application.DTOs.Activity;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ActivityEndpoints
    {
        public static void MapActivityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activity").RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await service.GetAllAsync(userId);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, IActivityService service) =>
            {
                var task = await service.GetByIdAsync(id);
                return task is null ? Results.NotFound() : Results.Ok(task);
            });

            group.MapPost("/", async (CreateActivityDto dto, IActivityService service) =>
            {
                var createdActivity = await service.CreateActivityAsync(dto);
                return Results.Created($"/api/activity/{createdActivity.Id}", createdActivity);
            });

            group.MapPut("/{id:guid}", async (Guid id, UpdateActivityDto dto, ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.UpdateActivityAsync(id, dto, userId);
                return Results.NoContent();
            });

            group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, IActivityService service) =>
            {
                var activity = await service.GetByIdAsync(id);
                if (activity is null)
                    return Results.NotFound(new { error = "Tarefa não encontrada." });

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (activity.Project?.UserId != userId)
                    return Results.Forbid();

                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
