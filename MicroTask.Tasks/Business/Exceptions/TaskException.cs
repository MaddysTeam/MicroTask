using System;

namespace Business
{

    public class TaskException : Exception
    {

        public TaskException() { }

        public TaskException(string message) : base(message) { }

        public TaskException(string message, Exception inner) : base(message, inner) { }

    }

}
