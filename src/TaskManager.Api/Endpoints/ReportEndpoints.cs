using System.Security.Claims;
using TaskManager.Application.DTOs.User;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/reports")
                .WithTags("Relatórios")
                .RequireAuthorization();

            group.MapGet("/performance", async (ClaimsPrincipal user, IReportService reportService) =>
            {
                var role = user.FindFirst(ClaimTypes.Role)?.Value;

                if (!string.Equals(role, "gerente", StringComparison.OrdinalIgnoreCase))
                    return Results.Forbid();

                List<UserTaskReportDto> report = await reportService.GetUserPerformanceReportAsync();
                return Results.Ok(report);

            })
            .WithName("GetUserPerformanceReport")
            .WithSummary("Relatório de desempenho dos usuários")
            .WithDescription("Média de tarefas concluídas nos últimos 30 dias. Apenas para gerentes.")
            .Produces<List<UserTaskReportDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}
