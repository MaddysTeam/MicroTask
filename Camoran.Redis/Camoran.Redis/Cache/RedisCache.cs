using System;
using System.Collections.Generic;
using System.Text;

namespace Camoran.Redis.Cache
{

    public interface ICahceStrategy<Key, Value>
    {
        void Set(Key key, Value val, TimeSpan? expireTime);
        Value Get(Key key);
        bool Remove(Key key);
        void SetExpire(Key key, TimeSpan expireTime);
    }


    public class RedisCache<Key, Value>
    {
        ICahceStrategy<Key, Value> _strategy;

        public RedisCache() { }

        public RedisCache(ICahceStrategy<Key, Value> strategy)
        {
            this._strategy = strategy ?? throw new RedisCacheException(ErrorInfo.STRATEGY_NOT_ALLOWED_NULL);
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

        public RedisStringCache(ICahceStrategy<string, string> strategy) : base(strategy) { }

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
