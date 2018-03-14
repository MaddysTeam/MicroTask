using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class TasktException : Exception
    {

        public TasktException() { }

        public TasktException(string message) : base(message) { }

        public TasktException(string message, Exception inner) : base(message, inner) { }

    }

}
