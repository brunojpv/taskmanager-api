using System.Security.Claims;
using TaskManager.Application.DTOs.Project;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/projects").RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await service.GetAllAsync(userId);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, IProjectService service) =>
            {
                var project = await service.GetByIdAsync(id);
                return project is null ? Results.NotFound() : Results.Ok(project);
            });

            group.MapPost("/", async (ClaimsPrincipal user, CreateProjectDto dto, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var createdProject = await service.CreateActivityAsync(dto, userId);
                return Results.Created($"/api/projects/{createdProject.Id}", createdProject);
            });

            group.MapPut("/{id:guid}", async (Guid id, ClaimsPrincipal user, UpdateProjectDto dto, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.UpdateProjectAsync(id, dto, userId);
                return Results.NoContent();
            });

            group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.DeleteProjectAsync(id, userId);
                return Results.NoContent();
            });
        }
    }
}
