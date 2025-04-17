using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;
using DomainTaskStatus = TaskManager.Domain.Entities.TaskStatus;

namespace TaskManager.IntegrationTests
{
    public class TaskManagerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;
        private Guid _projectId;
        private Guid _userId;
        private bool _disposed;

        public TaskManagerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        [Fact]
        public async Task DeletingProject_ShouldCascadeDeleteTasks()
        {
            var user = new User
            {
                Name = "Seed User",
                Email = "cascade@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _userId = user.Id;

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "super-secret-test-key",
                    ["Jwt:Issuer"] = "TaskManagerAPI",
                    ["Jwt:Audience"] = "TaskManagerUsers"
                })
                .Build();

            var authService = new AuthService(new UserRepository(_context), config);
            var loginToken = await authService.RegisterAsync(new Application.DTOs.RegisterRequest
            {
                Name = user.Name,
                Email = user.Email,
                Password = "123456"
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken.Token);

            var createProjectResponse = await _client.PostAsJsonAsync("/api/projects", new
            {
                name = "Projeto Cascade",
                description = "Será deletado"
            });
            var project = await createProjectResponse.Content.ReadFromJsonAsync<Project>();
            _projectId = project!.Id;

            for (int i = 0; i < 2; i++)
            {
                var createTaskResponse = await _client.PostAsJsonAsync("/api/tasks", new
                {
                    title = $"Tarefa {i + 1}",
                    description = "Para deletar em cascata",
                    dueDate = DateTime.UtcNow.AddDays(1),
                    status = DomainTaskStatus.Pending,
                    projectId = _projectId
                });
                createTaskResponse.EnsureSuccessStatusCode();
            }

            var tasksBeforeDelete = await _context.Tasks.Where(t => t.ProjectId == _projectId).ToListAsync();
            Assert.Equal(2, tasksBeforeDelete.Count);

            var deleteResponse = await _client.DeleteAsync($"/api/projects/{_projectId}");
            deleteResponse.EnsureSuccessStatusCode();

            var tasksAfterDelete = await _context.Tasks.Where(t => t.ProjectId == _projectId).ToListAsync();
            Assert.Empty(tasksAfterDelete);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            if (_userId != Guid.Empty)
            {
                var user = _context.Users
                    .Include(u => u.Projects)
                    .ThenInclude(p => p.Tasks)
                    .FirstOrDefault(u => u.Id == _userId);

                if (user is not null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class AuthResponse
    {
        public required string Token { get; set; }
    }
}
