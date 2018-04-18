using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{

    public static class RedisSessionConfig
    {

        public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
        {
            // add redis session
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = configuration.GetSection("redissessionsettings:conn").Value;
                option.InstanceName = "master";
            });


            services.AddSession();
            return services;
        }

    }

}
