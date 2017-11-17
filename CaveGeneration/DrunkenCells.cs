using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration
{
    class DrunkenCells
    {
        public int Width;
        public int Height;

        int[,] map;

        int numberOfWalks = 20;
        int numberOfSteps = 5000;
        int numberofSmoothings = 7;
        int distanceBetweenWalks = 7;

        Random rand = new Random();

        /* 
        public DIRECTION WeightedDirection;
        public int Weight;       
        public enum DIRECTION
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        */

        public DrunkenCells(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int[,] GetMap()
        {
            return map;
        }


        public void Start()
        {
            GenerateMap();
        }



        #region Private methods below


        private void GenerateMap()
        {
            map = new int[Width, Height];
            FillMap();
            MultiWalk(numberOfWalks);

            for (int i = 0; i < numberofSmoothings; i++)
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

        private void MultiWalk(int walkers)
        {
            int x = 0;
            int y = 0;

            for (int i = 0; i < walkers; i++)
            {
                Walk(x, y);
                y += distanceBetweenWalks;
            }
        }
        private void Walk(int StartX, int StartY)
        {


            for (int i = 0; i < numberOfSteps; i++)
            {
                StartX = MathHelper.Clamp(StartX, 0, Width - 1);
                StartY = MathHelper.Clamp(StartY, 0, Height - 1);

                int rnd = rand.Next(9);
                switch (rnd)
                {
                    case 0:
                    case 1:
                        if (StartY > 0)
                            map[StartX, StartY--] = 0;
                        break;
                    case 2:
                    case 3:
                        if (StartY < Height)
                            map[StartX, StartY++] = 0;
                        break;
                    case 4:
                    case 5:
                        if (StartX > 0)
                            map[StartX--, StartY] = 0;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        if (StartX < Width)
                            map[StartX++, StartY] = 0;
                        break;
                }
            }
            Console.WriteLine();
        }




        private void SmoothMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSorroundingWallCount(x, y);

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
#endregion

    }
}
