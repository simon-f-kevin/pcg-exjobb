using CaveGeneration.Content_Generation.Parameter_Settings;
using CaveGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Cleanup
{
    public class CellularAutomata : MapCleanup {
        
        private bool useCopy;


        public CellularAutomata(int width, int height, Settings settings)
        {
            this.Width = width;
            this.Height = height;
            this.settings = settings;
            useCopy = settings.usecopy;
        }

        public override int[,] SmoothMap(int[,] map, int iterationsOfSmoothmap)
        {
            
            for (int i = 0; i < iterationsOfSmoothmap; i++) {
                map = Smoothmap2(map);
            }
            return map;
        }

        private int[,] Smoothmap2(int[,] map)
        {
            var pm = CopyMap(map);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles;
                    if (useCopy)
                    {
                        neighbourWallTiles = GetSorroundingWallCount(x, y, pm);
                    }
                    else
                    {
                        neighbourWallTiles = GetSorroundingWallCount(x, y, map);
                    }

                    if (neighbourWallTiles > 4)
                    {
                        map[x, y] = 1;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        map[x, y] = 0;
                    }
                }
            }
            return map;
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
