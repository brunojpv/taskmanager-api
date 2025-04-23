using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.Api.Endpoints;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Application.Settings;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using TaskManager.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application/Domain services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityHistoryService, ActivityHistoryService>();
builder.Services.AddScoped<IActivityHistoryRepository, ActivityHistoryRepository>();
builder.Services.AddScoped<IActivityCommentService, ActivityCommentService>();
builder.Services.AddScoped<IActivityCommentRepository, ActivityCommentRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();
//builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"❌ Falha na autenticação: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"✅ Token validado para: {context.Principal?.Identity?.Name ?? "usuário desconhecido"}");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

    var baseDir = AppContext.BaseDirectory;
    opt.IncludeXmlComments(Path.Combine(baseDir, "TaskManager.Api.xml"), includeControllerXmlComments: true);
    opt.IncludeXmlComments(Path.Combine(baseDir, "TaskManager.Application.xml"));
    opt.IncludeXmlComments(Path.Combine(baseDir, "TaskManager.Domain.xml"));
    opt.IncludeXmlComments(Path.Combine(baseDir, "TaskManager.Infrastructure.xml"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(dbContext);
}

// Middlewares
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
    options.DocumentTitle = "TaskManager - API Docs";
    options.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

// Endpoint Mappings
app.MapAuthEndpoints();
app.MapProjectEndpoints();
app.MapActivityEndpoints();
app.MapActivityHistoryEndpoints();
app.MapActivityCommentEndpoints();
app.MapReportEndpoints();

await app.RunAsync();