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
            distanceBetweenWalks = 5,
            numberOfSteps = 250,
            numberOfWalks = 5,
            randomFillPercent = 45,
            mapGeneratorType = Content_Generation.Map_Generation.MapGeneratorType.DrunkardWalk
        };
    }
}
