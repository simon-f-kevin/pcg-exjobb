using CaveGeneration.Models;
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

        public void Draw(Grid grid)
        {
            if(map != null)
            {
                for(int x = 0; x < Width; x++)
                {
                    for(int y = 0; y < Height; y++)
                    {

                    }
                }
            }
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
        private void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = DateTime.Now.ToString();
            }

            Random pseudoRandom = new Random(Seed.GetHashCode());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
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

        private void SmoothMap()
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSorroundingWallCount(x, y);

                    if(neighbourWallTiles > 4)
                    {
                        map[x, y] = 1;
                    }
                    else if(neighbourWallTiles < 4)
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }

        private int GetSorroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

    }
}
