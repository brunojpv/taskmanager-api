using System.Security.Claims;
using TaskManager.Application.DTOs.Activity;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ActivityEndpoints
    {
        public static void MapActivityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activity")
                .WithTags("Atividades")
                .RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await service.GetAllAsync(userId);
                return Results.Ok(result);
            })
            .WithName("GetAllActivities")
            .WithSummary("Lista todas as atividades do usuário")
            .WithDescription("Retorna todas as atividades de todos os projetos vinculados ao usuário autenticado.")
            .Produces(StatusCodes.Status200OK);

            group.MapGet("/{id:guid}", async (Guid id, IActivityService service) =>
            {
                var task = await service.GetByIdAsync(id);
                return task is null ? Results.NotFound() : Results.Ok(task);
            })
            .WithName("GetActivityById")
            .WithSummary("Busca uma atividade pelo ID")
            .WithDescription("Retorna os detalhes de uma atividade específica pelo identificador único.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (CreateActivityDto dto, IActivityService service) =>
            {
                var createdActivity = await service.CreateActivityAsync(dto);
                return Results.Created($"/api/activity/{createdActivity.Id}", createdActivity);
            })
            .WithName("CreateActivity")
            .WithSummary("Cria uma nova atividade")
            .WithDescription("Cria uma nova tarefa dentro de um projeto específico.")
            .Produces(StatusCodes.Status201Created);

            group.MapPut("/{id:guid}", async (Guid id, UpdateActivityDto dto, ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.UpdateActivityAsync(id, dto, userId);
                return Results.NoContent();
            })
            .WithName("UpdateActivity")
            .WithSummary("Atualiza uma atividade")
            .WithDescription("Atualiza o título, descrição, status ou prioridade de uma atividade existente.")
            .Produces(StatusCodes.Status204NoContent);

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
            })
            .WithName("DeleteActivity")
            .WithSummary("Remove uma atividade")
            .WithDescription("Remove uma atividade desde que o usuário autenticado seja o dono do projeto.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
