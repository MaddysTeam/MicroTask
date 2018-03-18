using System;

namespace Infrastructure
{

    public class TasktException : Exception
    {

        public TasktException() { }

        public TasktException(string message) : base(message) { }

        public TasktException(string message, Exception inner) : base(message, inner) { }

    }

}
