using System.Security.Claims;
using TaskManager.Api.Extensions;
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
                var userId = user.GetUserId();
                if (userId is null)
                    return Results.Unauthorized();

                var result = await service.GetAllActivityAsync(userId);
                return Results.Ok(result);
            })
            .WithName("GetAllActivities")
            .WithSummary("Lista todas as atividades do usuário")
            .WithDescription("Retorna todas as atividades de todos os projetos vinculados ao usuário autenticado.")
            .Produces<IEnumerable<ActivityDto>>(StatusCodes.Status200OK);

            group.MapGet("/{id:guid}", async (Guid id, IActivityService service) =>
            {
                var result = await service.GetByIdAsync(id);
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .WithName("GetActivityById")
            .WithSummary("Busca uma atividade pelo ID")
            .WithDescription("Retorna os detalhes de uma atividade específica pelo identificador único.")
            .Produces<ActivityDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (CreateActivityDto dto, IActivityService service) =>
            {
                var result = await service.CreateActivityAsync(dto);
                return Results.Created($"/api/activity/{result.Id}", result);
            })
            .WithName("CreateActivity")
            .WithSummary("Cria uma nova atividade")
            .WithDescription("Cria uma nova tarefa dentro de um projeto específico.")
            .Produces<ActivityDto>(StatusCodes.Status201Created);

            group.MapPut("/{activityId:guid}", async (Guid activityId, UpdateActivityDto dto, ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = user.GetUserId();
                if (userId is null)
                    return Results.Unauthorized();

                var result = await service.UpdateActivityAsync(activityId, dto, userId);
                return result is null ? Results.Forbid() : Results.Ok(result);
            })
            .WithName("UpdateActivity")
            .WithSummary("Atualiza uma atividade")
            .WithDescription("Atualiza o título, descrição, status ou prioridade de uma atividade existente.")
            .Produces<ActivityDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

            group.MapDelete("/{activityId:guid}", async (Guid activityId, ClaimsPrincipal user, IActivityService service) =>
            {
                var userId = user.GetUserId();
                if (userId is null)
                    return Results.Unauthorized();

                var result = await service.DeleteActivityAsync(activityId, userId);
                return result ? Results.NoContent() : Results.Forbid();
            })
            .WithName("DeleteActivity")
            .WithSummary("Remove uma atividade")
            .WithDescription("Remove uma atividade desde que o usuário autenticado seja o dono do projeto.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}
