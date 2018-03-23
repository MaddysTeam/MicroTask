using Camoran.Redis.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Cache.Redis
{

    public static class RedisCacheConfig
    {

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheConfig = new RedisCacheConfiguration()
            {
                ConnectString = configuration.GetSection("RedisCacheSettings:conn").Value,
                DB = int.Parse(configuration.GetSection("RedisCacheSettings:db").Value)
            };
            services.AddSingleton(redisCacheConfig);
            services.AddTransient<IRedisCacheStrategy<string, string>, RedisEncryptKeyStrategy<string>>();
            services.AddTransient(typeof(RedisStringCache));
            services.AddTransient<IRedisCache, RedisCache>();

            return services;
        }

    }

}
