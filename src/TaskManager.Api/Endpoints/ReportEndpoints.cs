using TaskManager.Application.Services;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;

namespace TaskManager.Api.Endpoints
{
    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/reports").WithTags("Reports");

            group.MapGet("/performance/team", async (int days, IReportService reportService, HttpContext context) =>
            {
                try
                {
                    var report = await reportService.GetTeamPerformanceReportAsync(days);
                    return Results.Ok(report);
                }
                catch (DomainException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetTeamPerformanceReport")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter relatório de desempenho da equipe";
                operation.Description = "Retorna estatísticas de desempenho da equipe nos últimos dias especificados (acessível apenas para gerentes)";
                return operation;
            });

            group.MapGet("/user/{userId:guid}", async (Guid userId, int days, IReportService reportService, IUserRepository userRepository) =>
            {
                try
                {
                    var requestingUser = await userRepository.GetByIdAsync(userId);
                    if (requestingUser == null || !requestingUser.IsManager)
                    {
                        return Results.Forbid();
                    }

                    var report = await reportService.GetUserTaskReportAsync(userId, days);
                    return Results.Ok(report);
                }
                catch (DomainException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetUserTaskReport")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter relatório de tarefas de um usuário";
                operation.Description = "Retorna estatísticas detalhadas das tarefas de um usuário específico (acessível apenas para gerentes ou o próprio usuário)";
                return operation;
            });

            group.MapGet("/users/all", async (int days, IReportService reportService, IUserRepository userRepository, HttpContext context) =>
            {
                try
                {
                    var reports = await reportService.GetAllUsersTaskReportAsync(days);
                    return Results.Ok(reports);
                }
                catch (DomainException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetAllUsersTaskReport")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter relatório de tarefas de todos os usuários";
                operation.Description = "Retorna estatísticas detalhadas das tarefas de todos os usuários (acessível apenas para gerentes)";
                return operation;
            });
        }
    }
}
