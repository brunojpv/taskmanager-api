﻿using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/register", async (
                RegisterRequest request,
                IAuthService authService) =>
            {
                try
                {
                    var response = await authService.RegisterAsync(request);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            app.MapPost("/api/auth/login", async (
                LoginRequest request,
                IAuthService authService) =>
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
            });
        }
    }
}
