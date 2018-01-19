using CaveGeneration.Content_Generation.Parameter_Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Generation
{
    public enum MapGeneratorType
    {
        DrunkardWalk, 
        RandomPlacement,
    }

    public abstract class MapGenerator
    {
        public int Width;
        public int Height;

        public int[,] map;

        public bool UseRandomSeed;
        public string Seed;

        protected Settings settings;


        public MapGenerator(int width, int height, Settings settings)
        {
            Height = height;
            Width = width;
            this.settings = settings;
        }

        public abstract void Start(string seed);

        public int[,] GetMap()
        {
            return map;
        }

    }
}
