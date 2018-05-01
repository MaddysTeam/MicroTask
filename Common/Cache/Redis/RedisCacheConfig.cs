using Camoran.Redis.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cache.Redis
{

    public static class RedisCacheConfig
    {

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IRedisCacheStrategy, RedisEncryptKeyStrategy>();
            services.AddTransient<IRedisCacheManager, RedisCacheManager>();

            return services;
        }

    }

}
