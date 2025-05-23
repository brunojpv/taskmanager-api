﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Api.Tests.Helpers
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public IServiceCollection Services { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                Services = services;

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TaskManagerDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<TaskManagerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TaskManagerDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeTestDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void InitializeTestDatabase(TaskManagerDbContext context)
        {
            var testUser = new User(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Test User",
                "test@example.com");

            var managerUser = new User(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                "Test Manager",
                "manager@example.com",
                true);

            context.Users.Add(testUser);
            context.Users.Add(managerUser);
            context.SaveChanges();
        }

        public async Task<Project> AddTestProject(Guid userId, string name, string description)
        {
            using var scope = Services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

            var project = new Project(name, description, userId);
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return project;
        }

        public async Task<TaskItem> AddTestTask(Guid projectId, string title, string description, TaskPriority priority = TaskPriority.Medium)
        {
            using var scope = Services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

            var project = await db.Projects.FindAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with ID {projectId} not found");

            var task = new TaskItem(title, description, DateTime.Now.AddDays(1), priority, projectId);
            project.AddTask(task);

            await db.SaveChangesAsync();
            return task;
        }
    }
}
