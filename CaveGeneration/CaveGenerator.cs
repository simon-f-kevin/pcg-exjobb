using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration
{
    public class CaveGenerator
    {
        public int Width;
        public int Height;

        public string Seed;
        public bool UseRandomSeed;

        public int randomFillPercent;

        int[,] map;


        public void Start()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            map = new int[Width, Height];
            RandomFillMap();

            for(int i = 0; i < 5; i++)
            {
                SmoothMap();
            }
        }

        private void SmoothMap()
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {

                }
            }
        }

        private void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = DateTime.Now.ToString();
            }

            Random pseudoRandom = new Random(Seed.GetHashCode());

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width-1 || y == 0 || y == Height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
        }
    }
}
