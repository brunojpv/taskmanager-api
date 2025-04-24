using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Data.Seed;

namespace TaskManager.Infrastructure.Extensions
{
    public static class SeedFactoryExtensions
    {
        public static IServiceCollection AddSeedFactory(this IServiceCollection services)
        {
            services.AddScoped<SeedFactory>();
            return services;
        }
    }
}
