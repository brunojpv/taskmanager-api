using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ActivityHistoryEndpoints
    {
        public static void MapActivityHistoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/activity-history").RequireAuthorization();

            group.MapGet("/", async (Guid activityId, IActivityHistoryService service) =>
            {
                var history = await service.GetHistoryByActivityIdAsync(activityId);
                return Results.Ok(history);
            });
        }
    }
}
