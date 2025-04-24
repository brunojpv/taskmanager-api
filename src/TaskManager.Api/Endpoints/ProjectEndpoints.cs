using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Api.Endpoints
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/projects").WithTags("Projects");

            group.MapGet("/{userId:guid}", async (Guid userId, IProjectService projectService) =>
            {
                try
                {
                    var projects = await projectService.GetAllByUserIdAsync(userId);
                    return Results.Ok(projects);
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
            .WithName("GetProjectsByUserId")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter todos os projetos de um usuário";
                operation.Description = "Retorna todos os projetos associados a um usuário específico";
                return operation;
            });

            group.MapGet("/{id:guid}/details", async (Guid id, IProjectService projectService) =>
            {
                try
                {
                    var project = await projectService.GetByIdAsync(id);
                    return Results.Ok(project);
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
            .WithName("GetProjectById")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Obter detalhes de um projeto";
                operation.Description = "Retorna os detalhes de um projeto específico pelo ID";
                return operation;
            });

            group.MapPost("/", async (ProjectCreateDTO projectDto, IProjectService projectService) =>
            {
                try
                {
                    var project = await projectService.CreateAsync(projectDto);
                    return Results.Created($"/api/projects/{project.Id}/details", project);
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
            .WithName("CreateProject")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Criar um novo projeto";
                operation.Description = "Cria um novo projeto com os dados fornecidos";
                return operation;
            });

            group.MapPut("/", async (ProjectUpdateDTO projectDto, IProjectService projectService) =>
            {
                try
                {
                    var project = await projectService.UpdateAsync(projectDto);
                    return Results.Ok(project);
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
            .WithName("UpdateProject")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Atualizar um projeto";
                operation.Description = "Atualiza os dados de um projeto existente";
                return operation;
            });

            group.MapDelete("/{id:guid}", async (Guid id, IProjectService projectService) =>
            {
                try
                {
                    var result = await projectService.DeleteAsync(id);
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
            .WithName("DeleteProject")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Remover um projeto";
                operation.Description = "Remove um projeto existente pelo ID";
                return operation;
            });
        }
    }
}
