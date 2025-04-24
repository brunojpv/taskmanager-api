using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Endpoints
{
    public static class TaskEndpoints
    {
        public static void MapTaskEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/tasks").WithTags("Tasks");

            group.MapGet("/project/{projectId:guid}", async (Guid projectId, ITaskService taskService) =>
            {
                try
                {
                    var tasks = await taskService.GetAllByProjectIdAsync(projectId);
                    return Results.Ok(tasks);
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
            .WithName("GetTasksByProjectId")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter todas as tarefas de um projeto";
                operation.Description = "Retorna todas as tarefas associadas a um projeto específico";
                return operation;
            });

            group.MapGet("/{id:guid}", async (Guid id, ITaskService taskService) =>
            {
                try
                {
                    var task = await taskService.GetByIdAsync(id);
                    return Results.Ok(task);
                }
                catch (DomainException ex)
                {
                    return Results.NotFound(new { message = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetTaskById")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter detalhes de uma tarefa";
                operation.Description = "Retorna os detalhes de uma tarefa específica pelo ID";
                return operation;
            });

            group.MapPost("/", async (TaskCreateDTO taskDto, ITaskService taskService) =>
            {
                try
                {
                    var task = await taskService.CreateAsync(taskDto);
                    return Results.Created($"/api/tasks/{task.Id}", task);
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
            .WithName("CreateTask")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Criar uma nova tarefa";
                operation.Description = "Cria uma nova tarefa com os dados fornecidos";
                return operation;
            });

            group.MapPut("/", async (TaskUpdateDTO taskDto, ITaskService taskService) =>
            {
                try
                {
                    var task = await taskService.UpdateAsync(taskDto);
                    return Results.Ok(task);
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
            .WithName("UpdateTask")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Atualizar uma tarefa";
                operation.Description = "Atualiza os dados de uma tarefa existente";
                return operation;
            });

            group.MapDelete("/{id:guid}", async (Guid id, ITaskService taskService) =>
            {
                try
                {
                    var result = await taskService.DeleteAsync(id);
                    return result ? Results.NoContent() : Results.NotFound();
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
            .WithName("DeleteTask")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Remover uma tarefa";
                operation.Description = "Remove uma tarefa existente pelo ID";
                return operation;
            });

            group.MapGet("/{id:guid}/history", async (Guid id, ITaskService taskService) =>
            {
                try
                {
                    var history = await taskService.GetHistoryAsync(id);
                    return Results.Ok(history);
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
            .WithName("GetTaskHistory")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter histórico de uma tarefa";
                operation.Description = "Retorna o histórico de alterações de uma tarefa específica";
                return operation;
            });

            MapTaskCommentEndpoints(group);
        }

        private static void MapTaskCommentEndpoints(IEndpointRouteBuilder routes)
        {
            routes.MapPost("/comment", async (TaskCommentCreateDTO commentDto, ITaskService taskService) =>
            {
                try
                {
                    var task = await taskService.AddCommentAsync(commentDto);
                    return Results.Ok(task);
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
            .WithName("AddTaskComment")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Adicionar comentário a uma tarefa";
                operation.Description = "Adiciona um novo comentário a uma tarefa existente";
                return operation;
            });
        }
    }
}
