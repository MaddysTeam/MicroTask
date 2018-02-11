using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class CachServiceException : Exception
    {

        public CachServiceException() { }

        public CachServiceException(string message) : base(message) { }

        public CachServiceException(string message, Exception inner) : base(message, inner) { }

    }

}
