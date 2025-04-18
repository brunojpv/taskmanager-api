﻿using TaskManager.Infrastructure.Data.Builders;

namespace TaskManager.UnitTests.Builders
{
    public class ProjectBuilderTests
    {
        [Fact]
        public void Should_Create_Project_With_User()
        {
            var user = new UserBuilder()
                .WithName("Dev")
                .WithEmail("dev@test.com")
                .WithPassword("123")
                .Build();

            var project = new ProjectBuilder()
                .WithName("Project A")
                .WithDescription("Test project")
                .WithUser(user)
                .Build();

            Assert.NotNull(project);
            Assert.Equal("Project A", project.Name);
            Assert.Equal("Test project", project.Description);
            Assert.Equal(user, project.User);
        }

        [Fact]
        public void Should_Not_Allow_More_Than_20_Activities()
        {
            var project = new ProjectBuilder()
                .WithName("Limite")
                .Build();

            for (int i = 0; i < 20; i++)
            {
                project.Activities.Add(new ActivityBuilder()
                    .WithTitle($"Tarefa {i}")
                    .WithProject(project)
                    .Build());
            }

            Assert.False(project.CanAddNewActivity());
        }
    }
}
