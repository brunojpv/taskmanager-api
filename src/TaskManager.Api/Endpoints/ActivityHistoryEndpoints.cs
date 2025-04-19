using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ActivityHistoryEndpoints
    {
        public static void MapActivityHistoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activities/{activityId}/history")
                .WithTags("Histórico de Atividades")
                .RequireAuthorization();

            group.MapGet("/", async (Guid activityId, IActivityHistoryService service) =>
            {
                var history = await service.GetHistoryByActivityIdAsync(activityId);
                return Results.Ok(history);
            })
            .WithName("GetActivityHistory")
            .WithSummary("Lista o histórico de alterações da atividade")
            .WithDescription("Retorna todas as alterações registradas em uma atividade específica.")
            .Produces(StatusCodes.Status200OK);
        }
    }
}
