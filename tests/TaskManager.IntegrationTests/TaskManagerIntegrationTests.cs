using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TaskManager.Domain.Entities;

using DomainTaskStatus = TaskManager.Domain.Entities.TaskStatus;

namespace TaskManager.IntegrationTests
{
    public class TaskManagerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TaskManagerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FullUserProjectTaskFlow_Should_Succeed()
        {
            // Registro
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new
            {
                name = "Bruno",
                email = "integ@test.com",
                password = "123456"
            });
            var registerToken = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();

            // Login
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
            {
                email = "integ@test.com",
                password = "123456"
            });
            var loginToken = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken!.Token);

            // Criar Projeto
            var createProject = await _client.PostAsJsonAsync("/api/projects", new
            {
                name = "Projeto Integração",
                description = "Testando fluxo completo"
            });
            var project = await createProject.Content.ReadFromJsonAsync<Project>();

            // Criar Tarefa
            var createTask = await _client.PostAsJsonAsync("/api/tasks", new
            {
                title = "Criar testes",
                description = "Testes de integração",
                dueDate = DateTime.UtcNow.AddDays(5),
                status = DomainTaskStatus.Pending,
                projectId = project!.Id
            });
            createTask.EnsureSuccessStatusCode();

            // Validar listagem
            var tasks = await _client.GetAsync("/api/tasks");
            tasks.EnsureSuccessStatusCode();

            // Remover projeto
            var deleteProject = await _client.DeleteAsync($"/api/projects/{project.Id}");
            deleteProject.EnsureSuccessStatusCode();
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }
}
