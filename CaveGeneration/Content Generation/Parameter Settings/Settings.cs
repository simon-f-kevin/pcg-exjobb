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

        public int numberOfWalks { get; set; }

        public int numberOfSteps { get; set; }

        public int distanceBetweenWalks { get; set; }

        public int randomFillPercent { get; set; }


    }
}
