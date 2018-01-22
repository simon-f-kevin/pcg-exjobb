using CaveGeneration.Content_Generation.Map_Cleanup;
using CaveGeneration.Content_Generation.Parameter_Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Map_Generation
{
    public class DrunkardWalk : MapGenerator
    {

        int numberOfWalks = 5;
        int numberOfSteps = 250;
        int distanceBetweenWalks = 5;

        Random rand;

        Directions directions;

        /*
         * The odds of DrunkardWalk going in a certain direction
         * All ints must add up to 100
         */
        public class Directions
        {
            public int Up { get; set; }
            public int Down { get; set; }
            public int Left { get; set; }
            public int Right { get; set; }
        }

        public DrunkardWalk(int width, int height, Settings settings) : base(width, height, settings)
        {
            numberOfSteps = settings.NumberOfSteps;
            numberOfWalks = settings.NumberOfWalks;
            distanceBetweenWalks = settings.DistanceBetweenWalks;
            directions = settings.DrunkardDirections;
        }

        public override void Start(string seed)
        {
            if (seed.Equals(""))
            {
                seed = DateTime.Now.ToString();
            }
                

            
            rand = new Random(seed.GetHashCode());
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
            int x = (int)settings.StartPositionForWalkers.X;
            int y = (int)settings.StartPositionForWalkers.Y;

            for (int i = 0; i < walkers; i++)
            {
                Walk(x, y);
                y += distanceBetweenWalks;
            }
        }
        private void Walk(int StartX, int StartY)
        {
            int upChance = directions.Up;       
            int downChance = directions.Down    + upChance;
            int leftChance = directions.Left    + downChance;
            int rightChance = directions.Right  + leftChance;

            for (int i = 0; i < numberOfSteps; i++)
            {
                StartX = MathHelper.Clamp(StartX, 0, Width - 1);
                StartY = MathHelper.Clamp(StartY, 0, Height - 1);

                int rnd = rand.Next(100);


                if (rnd < upChance)
                {
                    map[StartX, StartY--] = 0;
                }
                else if (rnd < downChance)
                {
                    map[StartX, StartY++] = 0;
                }
                else if (rnd < leftChance)
                {
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
