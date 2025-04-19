using System.Security.Claims;
using TaskManager.Application.DTOs.Project;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Endpoints
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/projects")
                .WithTags("Projetos")
                .RequireAuthorization();

            group.MapGet("/", async (ClaimsPrincipal user, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await service.GetAllAsync(userId);
                return Results.Ok(result);
            })
            .WithName("GetUserProjects")
            .WithSummary("Lista projetos do usuário")
            .WithDescription("Retorna todos os projetos associados ao usuário autenticado.")
            .Produces<List<Project>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapGet("/{id:guid}", async (Guid id, IProjectService service) =>
            {
                var project = await service.GetByIdAsync(id);
                return project is null ? Results.NotFound() : Results.Ok(project);
            })
            .WithName("GetProjectById")
            .WithSummary("Busca um projeto por ID")
            .WithDescription("Retorna os detalhes de um projeto específico pelo seu identificador.")
            .Produces<Project>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (ClaimsPrincipal user, CreateProjectDto dto, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var createdProject = await service.CreateActivityAsync(dto, userId);
                return Results.Created($"/api/projects/{createdProject.Id}", createdProject);
            })
            .WithName("CreateProject")
            .WithSummary("Cria um novo projeto")
            .WithDescription("Cria um projeto vinculado ao usuário logado.")
            .Produces<Project>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized);


            group.MapPut("/{id:guid}", async (Guid id, ClaimsPrincipal user, UpdateProjectDto dto, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.UpdateProjectAsync(id, dto, userId);
                return Results.NoContent();
            })
            .WithName("UpdateProject")
            .WithSummary("Atualiza um projeto")
            .WithDescription("Atualiza as informações de um projeto pertencente ao usuário autenticado.")
            .Produces(StatusCodes.Status204NoContent);

            group.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal user, IProjectService service) =>
            {
                var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await service.DeleteProjectAsync(id, userId);
                return Results.NoContent();
            })
            .WithName("DeleteProject")
            .WithSummary("Remove um projeto")
            .WithDescription("Remove um projeto se não houver tarefas pendentes associadas a ele.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
