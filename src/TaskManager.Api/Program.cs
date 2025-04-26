using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManager.Api.Endpoints;
using TaskManager.Domain.Exceptions;
using TaskManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API para gerenciamento de tarefas e projetos",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Bruno Vieira",
            Email = "brunojpv@gmail.com"
        }
    });
});

// Adiciona serviços do TaskManager
builder.Services.AddTaskManagerServices(builder.Configuration);

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
        c.RoutePrefix = string.Empty; // Para servir a UI do Swagger na raiz
        c.DefaultModelsExpandDepth(-1); // Oculta o esquema de modelo por padrão
    });
}

// Middleware para tratamento global de exceções
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (DomainException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
    }
    catch (Exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "Ocorreu um erro interno no servidor." });
    }
});

// Map endpoints
app.MapProjectEndpoints();
app.MapTaskEndpoints();
app.MapReportEndpoints();

// Ensure database is created and migrations are applied
await app.Services.InitializeDatabaseAsync();

await app.RunAsync();