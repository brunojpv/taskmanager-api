using TaskManager.Application.DTOs.User;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth")
                .WithTags("Autenticação");

            group.MapPost("/register", async (RegisterDto request, IAuthService authService) =>
            {
                var response = await authService.RegisterAsync(request);
                return Results.Ok(response);
            })
            .WithName("RegisterUser")
            .WithSummary("Registro de novo usuário")
            .WithDescription("Cria um novo usuário na plataforma com os dados fornecidos.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapPost("/login", async (LoginDto request, IAuthService authService) =>
            {
                try
                {
                    var response = await authService.LoginAsync(request);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.Json(new { error = ex.Message }, statusCode: StatusCodes.Status401Unauthorized);
                }
            })
            .WithName("LoginUser")
            .WithSummary("Login do usuário")
            .WithDescription("Realiza autenticação de um usuário e retorna um token JWT se as credenciais forem válidas.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        }
    }
}
