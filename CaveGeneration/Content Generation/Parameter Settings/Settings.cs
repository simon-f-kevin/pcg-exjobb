using CaveGeneration.Content_Generation.Map_Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Parameter_Settings
{
    public class Settings
    {
        public int EnemyCount { get; set; }

        public bool GoalonGround { get; set; }

        public int IterationsOfsmoothmap { get; set; }

        public bool usecopy { get; set; }

        public MapGeneratorType mapGeneratorType { get; set; }

        public int NumberOfWalks { get; set; }

        public int NumberOfSteps { get; set; }

        public int DistanceBetweenWalks { get; set; }

        public int RandomFillPercent { get; set; }

        public int PlayerLives { get; set; }


    }
}
