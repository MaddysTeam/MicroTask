using System;
using System.Collections.Generic;
using System.Text;

namespace Camoran.Redis.Cache
{

    [Serializable]
    public class RedisCacheException : Exception
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
