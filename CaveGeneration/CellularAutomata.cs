using CaveGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration
{
    public class CellularAutomata
    {
        public int Width;
        public int Height;

        public string Seed;
        public bool UseRandomSeed;

        public int randomFillPercent;

        int[,] map;

        public CellularAutomata(int width, int height, int randomFillPercent)
        {
            Width = width;
            Height = height;
            this.randomFillPercent = randomFillPercent;
        }

        public void Start(string seed)
        {
            if (seed.Equals(""))
                UseRandomSeed = true;

            Seed = seed;
            GenerateMap();
        }

        public int[,] GetMap()
        {
            return map;
        }

        private void GenerateMap()
        {
            map = new int[Width, Height];
            RandomFillMap();

            for(int i = 0; i < 3; i++)
            {
                SmoothMap();
            }
        }

        private void FillMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    map[x, y] = 1;
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
            var pm = CopyMap(map);
            for (int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSorroundingWallCount(x, y, map);

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

        private int[,] CopyMap(int[,] map)
        {
            int[,] previousMap = new int[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    previousMap[x, y] = map[x, y];
                }
            }

            return previousMap;
        }

        private int GetSorroundingWallCount(int gridX, int gridY, int[,] map)
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
