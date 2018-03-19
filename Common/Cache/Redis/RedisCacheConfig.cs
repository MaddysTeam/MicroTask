using Camoran.Redis.Cache;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
