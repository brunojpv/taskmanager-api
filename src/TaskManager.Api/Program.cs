using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManager.Api.Endpoints;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Exceptions;
using TaskManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTaskManagerServices(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
        c.DefaultModelsExpandDepth(-1);
    });
}

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

app.MapProjectEndpoints();
app.MapTaskEndpoints();
app.MapReportEndpoints();

await app.Services.InitializeDatabaseAsync();

await app.RunAsync();