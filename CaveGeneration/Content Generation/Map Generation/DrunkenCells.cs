using CaveGeneration.Content_Generation.Map_Cleanup;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Generation
{
    public class DrunkenCells : MapGenerator
    {

        int numberOfWalks = 10;
        int numberOfSteps = 10000;
        int numberofSmoothings = 3;
        int distanceBetweenWalks = 5;

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

        public DrunkenCells(int width, int height) : base(width, height)
        {

        }

        public override void Start(string seed)
        {
            if (seed.Equals(""))
                UseRandomSeed = true;

            Seed = seed;

            GenerateMap();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        map[x, y] = 1;
                    }
                }
            }

        }

        #region Private methods below


        private void GenerateMap()
        {
            map = new int[Width, Height];
            FillMap();
            MultiWalk(numberOfWalks);

            CellularAutomata ca = new CellularAutomata(Width, Height, map);

            for (int i = 0; i < numberofSmoothings; i++)
            {
                ca.SmoothMap();
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
            int upChance = 20;      //Ignore this column
            int downChance = 20     + upChance;
            int leftChance = 20     + downChance;
            int rightChance = 40    + leftChance;

            for (int i = 0; i < numberOfSteps; i++)
            {
                StartX = MathHelper.Clamp(StartX, 0, Width - 1);
                StartY = MathHelper.Clamp(StartY, 0, Height - 1);

                int rnd = rand.Next(100);


                if(rnd < upChance)
                {
                    map[StartX, StartY--] = 0;
                }
                else if (rnd < downChance)
                {
                    map[StartX, StartY++] = 0;
                }
                else if (rnd < leftChance){
                    map[StartX--, StartY] = 0;
                }
                else if (rnd < rightChance)
                {
                    map[StartX++, StartY] = 0;
                }

            }

        }
#endregion

    }
}
