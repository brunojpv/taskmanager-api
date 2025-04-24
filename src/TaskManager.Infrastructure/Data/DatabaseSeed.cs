using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data
{
    public static class DatabaseSeed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedUsers(modelBuilder);
            SeedProjects(modelBuilder);
            SeedTasks(modelBuilder);
            SeedTaskHistoryEntries(modelBuilder);
            SeedTaskComments(modelBuilder);
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = userId1,
                    Name = "Usuário Comum",
                    Email = "usuario@exemplo.com",
                    IsManager = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = (DateTime?)null
                },
                new
                {
                    Id = userId2,
                    Name = "Gerente",
                    Email = "gerente@exemplo.com",
                    IsManager = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = (DateTime?)null
                }
            );

            UserSeeds.UserId1 = userId1;
            UserSeeds.UserId2 = userId2;
        }

        private static void SeedProjects(ModelBuilder modelBuilder)
        {
            var projectId1 = Guid.NewGuid();
            var projectId2 = Guid.NewGuid();

            modelBuilder.Entity<Project>().HasData(
                new
                {
                    Id = projectId1,
                    Name = "Projeto Demo 1",
                    Description = "Este é um projeto de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = (DateTime?)null,
                    UserId = UserSeeds.UserId1
                },
                new
                {
                    Id = projectId2,
                    Name = "Projeto Demo 2",
                    Description = "Este é outro projeto de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = (DateTime?)null,
                    UserId = UserSeeds.UserId2
                }
            );

            ProjectSeeds.ProjectId1 = projectId1;
            ProjectSeeds.ProjectId2 = projectId2;
        }

        private static void SeedTasks(ModelBuilder modelBuilder)
        {
            var taskId1 = Guid.NewGuid();
            var taskId2 = Guid.NewGuid();

            modelBuilder.Entity<TaskItem>().HasData(
                new
                {
                    Id = taskId1,
                    Title = "Tarefa Demo 1",
                    Description = "Esta é uma tarefa de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = (DateTime?)null,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = TaskItemStatus.Pending,
                    Priority = TaskPriority.Medium,
                    ProjectId = ProjectSeeds.ProjectId1
                },
                new
                {
                    Id = taskId2,
                    Title = "Tarefa Demo 2",
                    Description = "Esta é outra tarefa de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(10),
                    Status = TaskItemStatus.InProgress,
                    Priority = TaskPriority.High,
                    ProjectId = ProjectSeeds.ProjectId2
                }
            );

            TaskSeeds.TaskId1 = taskId1;
            TaskSeeds.TaskId2 = taskId2;
        }

        private static void SeedTaskHistoryEntries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskHistoryEntry>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Action = "Tarefa criada",
                    Details = (string)null,
                    Timestamp = DateTime.UtcNow.AddDays(-9),
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId1,
                    UserId = (Guid?)null
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Action = "Tarefa criada",
                    Details = (string)null,
                    Timestamp = DateTime.UtcNow.AddDays(-4),
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId2,
                    UserId = (Guid?)null
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Action = "Status alterado",
                    Details = "Status alterado de Pending para InProgress",
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId2,
                    UserId = UserSeeds.UserId2
                }
            );
        }

        private static void SeedTaskComments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskComment>().HasData(
                new
                {
                    Id = Guid.NewGuid(),
                    Content = "Este é um comentário de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId1,
                    UserId = UserSeeds.UserId1
                },
                new
                {
                    Id = Guid.NewGuid(),
                    Content = "Este é outro comentário de demonstração",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId2,
                    UserId = UserSeeds.UserId2
                }
            );
        }

        private static class UserSeeds
        {
            public static Guid UserId1 { get; set; }
            public static Guid UserId2 { get; set; }
        }

        private static class ProjectSeeds
        {
            public static Guid ProjectId1 { get; set; }
            public static Guid ProjectId2 { get; set; }
        }

        private static class TaskSeeds
        {
            public static Guid TaskId1 { get; set; }
            public static Guid TaskId2 { get; set; }
        }
    }
}
