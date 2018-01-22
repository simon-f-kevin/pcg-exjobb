using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Parameter_Settings
{
    class PredefinedSettings
    {

        public static Settings settings1 = new Settings()
        {
            EnemyCount = 3,
            GoalonGround = true,
            usecopy = true,
            IterationsOfsmoothmap = 2,
            DistanceBetweenWalks = 5,
            NumberOfSteps = 250,
            NumberOfWalks = 5,
            RandomFillPercent = 45,
            mapGeneratorType = Content_Generation.Map_Generation.MapGeneratorType.DrunkardWalk,
            PlayerLives = 3
        };
    }
}
