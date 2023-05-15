using Microsoft.Extensions.DependencyInjection;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Infrastructure.Logging;

namespace TT.Deliveries.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}
