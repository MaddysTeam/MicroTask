using System;
using System.Collections.Generic;
using System.Text;

namespace Camoran.Redis.Cache
{

    public class RedisCache<Key, Value>
    {
        IRedisCacheStrategy<Key, Value> _strategy;

        public RedisCache(IRedisCacheStrategy<Key, Value> strategy, RedisCacheConfiguration config)
        {
            _strategy = strategy ?? throw new RedisCacheException(ErrorInfo.STRATEGY_NOT_ALLOWED_NULL);

            if (config != null)
                _strategy.SetConfig(config);
        }

        public virtual Value Get(Key key)
        {
            return _strategy.Get(key);
        }

        public virtual bool Remove(Key key)
        {
            return _strategy.Remove(key);
        }

        public virtual void Set(Key key, Value val, TimeSpan expireTime)
        {
            _strategy.Set(key, val, expireTime);
        }

        public virtual void SetExpire(Key key, TimeSpan expireTime)
        {
            _strategy.SetExpire(key, expireTime);
        }

    }

    public class RedisStringCache : RedisCache<string, string>
    {

        public RedisStringCache(IRedisCacheStrategy<string, string> strategy, RedisCacheConfiguration config) : base(strategy, config) { }

    }   


    [Serializable]
    internal class RedisCacheException : Exception
    {
        private string _message;

        public RedisCacheException() { }

        public RedisCacheException(string message) : base(message) => _message = message;

        public RedisCacheException(string message, Exception inner)
            : base(message, inner)
        { }
    }


    internal static class ErrorInfo
    {
        internal static string STRATEGY_NOT_ALLOWED_NULL = "cache strategy is not allowed null";
    }

}
