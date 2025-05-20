using Fiap.CloudGames.Fase1.API.Middleware.Logging;
using Fiap.CloudGames.Fase1.Infrastructure.Logging;

namespace Fiap.CloudGames.Fase1.API.Middleware
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddTransient<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }
    }
}
