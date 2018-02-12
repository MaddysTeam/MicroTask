using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class CacheServiceException : Exception
    {

        public CacheServiceException() { }

        public CacheServiceException(string message) : base(message) { }

        public CacheServiceException(string message, Exception inner) : base(message, inner) { }

    }

}
