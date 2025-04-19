using System.Security.Claims;
using TaskManager.Application.DTOs.ActivityComment;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Endpoints
{
    public static class ActivityCommentEndpoints
    {
        public static void MapActivityCommentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activities/{activityId}/comments")
                .WithTags("Comentários")
                .RequireAuthorization();

            group.MapPost("/", async (Guid activityId, AddCommentDto dto, ClaimsPrincipal user, IActivityCommentService commentService) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null) return Results.Unauthorized();

                dto.ActivityId = activityId;
                await commentService.AddCommentAsync(Guid.Parse(userId), dto);
                return Results.NoContent();
            })
            .WithName("AddCommentToActivity")
            .WithSummary("Adiciona um comentário a uma tarefa")
            .WithDescription("Permite que um usuário com permissão adicione um comentário a uma tarefa específica.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapGet("/", async (Guid activityId, IActivityCommentService commentService) =>
            {
                var comments = await commentService.GetCommentsByActivityIdAsync(activityId);
                return Results.Ok(comments);
            })
            .WithName("GetCommentsByActivity")
            .WithSummary("Lista os comentários de uma tarefa")
            .WithDescription("Retorna todos os comentários associados a uma tarefa específica.")
            .Produces<List<ActivityComment>>(StatusCodes.Status200OK);
        }
    }
}
