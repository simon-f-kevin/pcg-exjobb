using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Astar
{
    public class NotSolveableException : Exception
    {
        public NotSolveableException()
        {
        }

        public NotSolveableException(string message)
        : base(message)
        {
        }

        public NotSolveableException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
