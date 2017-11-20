using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation
{
    public abstract class MapGenerator
    {
        public int Width;
        public int Height;

        public int[,] map;

        public bool UseRandomSeed;
        public string Seed;

        public MapGenerator(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public abstract void Start(string seed);

        public int[,] GetMap()
        {
            return map;
        }

    }
}
