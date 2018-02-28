using Camoran.Redis.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IRedisCache
    {
        string Get(string key);
        bool IsExpire(string key);
        bool Set(string key, string value);
        bool Remove(string key);
    }

    public class RedisCache: IRedisCache
    {
        public RedisCache(RedisStringCache cache, IConfiguration configuration, ILoggerFactory factory)
        {
            this.cache = cache;

            logger = factory.CreateLogger<RedisCache>();

            expireMillonseconds = int.Parse(configuration.GetSection("RedisCacheSettings:expireMilliseconds").Value);
            expireHours = int.Parse(configuration.GetSection("RedisCacheSettings:expireHours").Value);
            expireMiutes = int.Parse(configuration.GetSection("RedisCacheSettings:expireMiutes").Value);
            expireSeconds = int.Parse(configuration.GetSection("RedisCacheSettings:expireSeconds").Value);
            expireDays = int.Parse(configuration.GetSection("RedisCacheSettings:expireDays").Value);
        }

        public string Get(string key)
        {
            try
            {
                return cache.Get(key);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return string.Empty;
            }
        }

        public bool IsExpire(string key)
        {
            return false;
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Set(string key, string value)
        {
            try
            {
                cache.Set(key, value,
                new TimeSpan(
                    expireDays,
                    expireHours,
                    expireMiutes,
                    expireSeconds,
                    expireMillonseconds));

                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return false;
            }
        }

        private RedisStringCache cache;
        private int expireMillonseconds = 0;
        private int expireHours = 0;
        private int expireMiutes = 0;
        private int expireSeconds = 0;
        private int expireDays = 0;
        private ILogger logger;
    }
}
