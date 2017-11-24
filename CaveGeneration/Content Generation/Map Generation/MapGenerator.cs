using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Generation
{
    public abstract class MapGenerator
    {
        public int Width;
        public int Height;

        public int[,] map;

        public bool UseRandomSeed;
        public string Seed;

        public MapGenerator(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public abstract void Start(string seed, int iterationsOfSmoothmap);

        public int[,] GetMap()
        {
            return map;
        }

    }
}
