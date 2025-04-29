using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Infrastructure.Data.Seed
{
    public static class DatabaseSeed
    {
        private static readonly DateTime BaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
            var userId1 = new Guid("11111111-1111-1111-1111-111111111111");
            var userId2 = new Guid("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = userId1,
                    Name = "Usuário Comum",
                    Email = "usuario@exemplo.com",
                    IsManager = false,
                    CreatedAt = BaseDate,
                    UpdatedAt = (DateTime?)null
                },
                new
                {
                    Id = userId2,
                    Name = "Gerente",
                    Email = "gerente@exemplo.com",
                    IsManager = true,
                    CreatedAt = BaseDate,
                    UpdatedAt = (DateTime?)null
                }
            );

            UserSeeds.UserId1 = userId1;
            UserSeeds.UserId2 = userId2;
        }

        private static void SeedProjects(ModelBuilder modelBuilder)
        {
            var projectId1 = new Guid("33333333-3333-3333-3333-333333333333");
            var projectId2 = new Guid("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<Project>().HasData(
                new
                {
                    Id = projectId1,
                    Name = "Projeto Demo 1",
                    Description = "Este é um projeto de demonstração",
                    CreatedAt = BaseDate.AddDays(-10),
                    UpdatedAt = (DateTime?)null,
                    UserId = UserSeeds.UserId1
                },
                new
                {
                    Id = projectId2,
                    Name = "Projeto Demo 2",
                    Description = "Este é outro projeto de demonstração",
                    CreatedAt = BaseDate.AddDays(-5),
                    UpdatedAt = (DateTime?)null,
                    UserId = UserSeeds.UserId2
                }
            );

            ProjectSeeds.ProjectId1 = projectId1;
            ProjectSeeds.ProjectId2 = projectId2;
        }

        private static void SeedTasks(ModelBuilder modelBuilder)
        {
            var taskId1 = new Guid("55555555-5555-5555-5555-555555555555");
            var taskId2 = new Guid("66666666-6666-6666-6666-666666666666");

            modelBuilder.Entity<TaskItem>().HasData(
                new
                {
                    Id = taskId1,
                    Title = "Tarefa Demo 1",
                    Description = "Esta é uma tarefa de demonstração",
                    CreatedAt = BaseDate.AddDays(-9),
                    UpdatedAt = (DateTime?)null,
                    DueDate = BaseDate.AddDays(5),
                    Status = TaskItemStatus.Pending,
                    Priority = TaskPriority.Medium,
                    ProjectId = ProjectSeeds.ProjectId1
                },
                new
                {
                    Id = taskId2,
                    Title = "Tarefa Demo 2",
                    Description = "Esta é outra tarefa de demonstração",
                    CreatedAt = BaseDate.AddDays(-4),
                    UpdatedAt = BaseDate.AddDays(-2),
                    DueDate = BaseDate.AddDays(10),
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
                    Id = new Guid("77777777-7777-7777-7777-777777777777"),
                    Action = "Tarefa criada",
                    Details = "Tarefa criada inicialmente",
                    Timestamp = BaseDate.AddDays(-9),
                    CreatedAt = BaseDate.AddDays(-9),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId1,
                    UserId = (Guid?)null
                },
                new
                {
                    Id = new Guid("88888888-8888-8888-8888-888888888888"),
                    Action = "Tarefa criada",
                    Details = "Tarefa inicial",
                    Timestamp = BaseDate.AddDays(-4),
                    CreatedAt = BaseDate.AddDays(-4),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId2,
                    UserId = (Guid?)null
                },
                new
                {
                    Id = new Guid("99999999-9999-9999-9999-999999999999"),
                    Action = "Status alterado",
                    Details = "Status alterado de Pending para InProgress",
                    Timestamp = BaseDate.AddDays(-2),
                    CreatedAt = BaseDate.AddDays(-2),
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
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Content = "Este é um comentário de demonstração",
                    CreatedAt = BaseDate.AddDays(-8),
                    UpdatedAt = (DateTime?)null,
                    TaskId = TaskSeeds.TaskId1,
                    UserId = UserSeeds.UserId1
                },
                new
                {
                    Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Content = "Este é outro comentário de demonstração",
                    CreatedAt = BaseDate.AddDays(-3),
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
