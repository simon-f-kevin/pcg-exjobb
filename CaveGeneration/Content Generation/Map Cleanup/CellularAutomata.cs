using CaveGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Cleanup
{
    public class CellularAutomata {
        private int Width;
        private int Height;
        private int[,] map;

        public CellularAutomata(int width, int height, int[,] Map)
        {
            this.Width = width;
            this.Height = height;
            this.map = Map;

        }

        public void SmoothMap()
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
