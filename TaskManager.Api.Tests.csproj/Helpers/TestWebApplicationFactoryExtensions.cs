using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;

namespace TaskManager.Api.Tests.Helpers
{
    public static class TestWebApplicationFactoryExtensions
    {
        public static void ReplaceService<T>(this TestWebApplicationFactory factory, T mockService) where T : class
        {
            factory.Services.AddOrReplace(mockService);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddOrReplace<T>(this IServiceCollection services, T implementation) where T : class
        {
            if (implementation == null)
                throw new ArgumentNullException(nameof(implementation));

            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(T));

            if (descriptorToRemove != null)
            {
                services.Remove(descriptorToRemove);
            }

            // Determine the correct service lifetime to use
            var lifetime = ServiceLifetime.Scoped; // Default lifetime
            if (descriptorToRemove != null)
            {
                lifetime = descriptorToRemove.Lifetime;
            }
            else if (typeof(IRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(IProjectRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(ITaskRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(IUserRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(IReportRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(ITaskHistoryRepository).IsAssignableFrom(typeof(T)) ||
                    typeof(ITaskCommentRepository).IsAssignableFrom(typeof(T)))
            {
                lifetime = ServiceLifetime.Scoped;
            }
            else if (typeof(IProjectService).IsAssignableFrom(typeof(T)) ||
                    typeof(ITaskService).IsAssignableFrom(typeof(T)) ||
                    typeof(IReportService).IsAssignableFrom(typeof(T)))
            {
                lifetime = ServiceLifetime.Scoped;
            }

            // Add the service with the appropriate lifetime
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(T), implementation);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(T), _ => implementation);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(T), _ => implementation);
                    break;
                default:
                    services.AddScoped(typeof(T), _ => implementation);
                    break;
            }
        }
    }

    public interface IRepository { }
}
