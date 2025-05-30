using Microsoft.Extensions.DependencyInjection;
using vv.Domain.Repositories;
using vv.Infrastructure.Factories;

namespace vv.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for registering services with DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add CQRS repositories to the service collection
        /// </summary>
        public static IServiceCollection AddMarketDataCqrs(this IServiceCollection services)
        {
            // Register factory
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

            // Register CQRS components
            services.AddScoped<IMarketDataQueries>(sp =>
            {
                var factory = sp.GetRequiredService<IRepositoryFactory>();
                return factory.CreateMarketDataQueries();
            });

            services.AddScoped<IMarketDataCommands>(sp =>
            {
                var factory = sp.GetRequiredService<IRepositoryFactory>();
                return factory.CreateMarketDataCommands();
            });

            return services;
        }
    }
}