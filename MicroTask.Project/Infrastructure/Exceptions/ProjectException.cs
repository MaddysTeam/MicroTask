using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{

    public class ProjectException : Exception
    {

        public ProjectException() { }

        public ProjectException(string message) : base(message) { }

        public ProjectException(string message, Exception inner) : base(message, inner) { }

    }

}
