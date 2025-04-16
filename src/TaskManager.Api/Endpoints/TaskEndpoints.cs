using System.Security.Claims;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Endpoints
{
    public static class TaskEndpoints
    {
        public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tasks").RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, ITaskService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!);
                var result = await service.GetAllAsync(userId);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, ITaskService service) =>
            {
                var task = await service.GetByIdAsync(id);
                return task is null ? Results.NotFound() : Results.Ok(task);
            });

            group.MapPost("/", async (
                CreateTaskRequest request,
                ITaskService service) =>
            {
                var task = new TaskItem
                {
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    Status = request.Status,
                    ProjectId = request.ProjectId
                };
                await service.AddAsync(task);
                return Results.Created($"/api/tasks/{task.Id}", task);
            });

            group.MapPut("/{id:guid}", async (
                Guid id,
                UpdateTaskRequest request,
                ITaskService service) =>
            {
                if (id != request.Id) return Results.BadRequest();

                var task = new TaskItem
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    DueDate = request.DueDate,
                    Status = request.Status,
                    ProjectId = request.ProjectId
                };
                await service.UpdateAsync(task);
                return Results.NoContent();
            });

            group.MapDelete("/{id:guid}", async (Guid id, ITaskService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
