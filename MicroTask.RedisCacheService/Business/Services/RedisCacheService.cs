using Camoran.Redis.Cache;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public class RedisStringCacheService : IRedisCacheStringKeyValueService
    {
        private RedisStringCache cache;
        private int expireMillonseconds = 0;
        private int expireHours = 0;
        private int expireMiutes = 0;
        private int expireSeconds = 0;
        private int expireDays = 0;

        public RedisStringCacheService(RedisStringCache cache, IConfiguration configuration)
        {
            this.cache = cache;
            expireMillonseconds = int.Parse(configuration.GetSection("CacheSettings:expireMillonseconds").Value);
            expireHours = int.Parse(configuration.GetSection("CacheSettings:expireHours").Value);
            expireMiutes = int.Parse(configuration.GetSection("CacheSettings:expireMiutes").Value);
            expireSeconds = int.Parse(configuration.GetSection("CacheSettings:expireSeconds").Value);
            expireDays = int.Parse(configuration.GetSection("CacheSettings:expireDays").Value);
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            return cache.Get(key);
        }

        public bool IsExpire(string key)
        {
            return false;
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void SetCache(string key, string value)
        {
            cache.Set(key, value,
                new TimeSpan(
                    expireDays,
                    expireHours, 
                    expireMiutes,
                    expireSeconds,
                    expireMillonseconds));
        }

    }

}
