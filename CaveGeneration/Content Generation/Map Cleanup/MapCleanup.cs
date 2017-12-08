using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Cleanup
{
    public abstract class MapCleanup
    {
        protected int Width;
        protected int Height;
        protected int[,] map;

        public abstract int[,] SmoothMap(int[,] map);

    }
}
