using Camoran.Redis.Cache;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Business
{

    public class RedisStringCacheService : IRedisCacheStringKeyValueService
    {

        public RedisStringCacheService(RedisStringCache cache, IConfiguration configuration, ILoggerFactory factory)
        {
            this.cache = cache;

            logger = factory.CreateLogger<RedisStringCacheService>();

            expireMillonseconds = int.Parse(configuration.GetSection("CacheSettings:expireMilliseconds").Value);
            expireHours = int.Parse(configuration.GetSection("CacheSettings:expireHours").Value);
            expireMiutes = int.Parse(configuration.GetSection("CacheSettings:expireMiutes").Value);
            expireSeconds = int.Parse(configuration.GetSection("CacheSettings:expireSeconds").Value);
            expireDays = int.Parse(configuration.GetSection("CacheSettings:expireDays").Value);
        }

        public string Get(string key)
        {
            try
            {
                return cache.Get(key);
            }
            catch(Exception e)
            {
                // log error
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

        public bool SetCache(string key, string value)
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
            catch(Exception e)
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
