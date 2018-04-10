using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{

    public static class MemorySessionConfig
    {

        public static IServiceCollection AddMemorySession(this IServiceCollection services, IConfiguration configuration)
        {
            // add memory session
            services.AddDistributedMemoryCache();
            services.AddSession();

            return services;
        }

    }

}
