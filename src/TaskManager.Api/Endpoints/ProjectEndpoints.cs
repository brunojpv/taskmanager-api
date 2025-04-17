using System.Security.Claims;
using TaskManager.Application.DTOs.Project;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

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

            group.MapPost("/", async (
                ClaimsPrincipal user,
                CreateProjectRequest request,
                IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var project = new Project
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = userId
                };
                await service.AddAsync(project);
                return Results.Created($"/api/projects/{project.Id}", project);
            });

            group.MapPut("/{id:guid}", async (
                Guid id,
                UpdateProjectRequest request,
                IProjectService service) =>
            {
                if (id != request.Id) return Results.BadRequest();

                var project = new Project
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description
                };
                await service.UpdateAsync(project);
                return Results.NoContent();
            });

            group.MapDelete("/{id:guid}", async (
                Guid id,
                ClaimsPrincipal user,
                IProjectService service) =>
            {
                var project = await service.GetByIdAsync(id);
                if (project is null)
                    return Results.NotFound(new { error = "Projeto não encontrado." });

                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                if (project.UserId != userId)
                    return Results.Forbid();

                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
