using Camoran.Redis.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Common.Cache.Redis
{

    public interface IRedisCacheManager
    {
        Value Get<Value>(string key);
        bool IsExpire(string key);
        bool Set<Value>(string key, Value value);
        bool Remove(string key);
    }

    public class RedisCacheManager: IRedisCacheManager
    {
        public RedisCacheManager(IRedisCacheStrategy cache, IConfiguration configuration, ILoggerFactory factory)
        {
            cache.EnsureNotNull(()=>new RedisCacheException());

            _cache = cache;
            _cache.SetConfig(new RedisCacheConfiguration()
            {
                ConnectString = configuration.GetSection("RedisCacheSettings:conn").Value,
                DB = int.Parse(configuration.GetSection("RedisCacheSettings:db").Value)
            });

            logger = factory.CreateLogger<RedisCacheManager>();

            expireMillonseconds = int.Parse(configuration.GetSection("RedisCacheSettings:expireMilliseconds").Value);
            expireHours = int.Parse(configuration.GetSection("RedisCacheSettings:expireHours").Value);
            expireMiutes = int.Parse(configuration.GetSection("RedisCacheSettings:expireMiutes").Value);
            expireSeconds = int.Parse(configuration.GetSection("RedisCacheSettings:expireSeconds").Value);
            expireDays = int.Parse(configuration.GetSection("RedisCacheSettings:expireDays").Value);
        }

        public Value Get<Value>(string key)
        {
            try
            {
                return _cache.Get<Value>(key);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return default(Value);
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

        public bool Set<Value>(string key, Value value)
        {
            try
            {
                _cache.Set(key, value,
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

        private IRedisCacheStrategy _cache;
        private int expireMillonseconds = 0;
        private int expireHours = 0;
        private int expireMiutes = 0;
        private int expireSeconds = 0;
        private int expireDays = 0;
        private ILogger logger;

    }

}
