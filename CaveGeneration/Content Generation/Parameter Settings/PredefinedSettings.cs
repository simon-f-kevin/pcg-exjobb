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
            //distance between enemeies
            GoalonGround = true,
            usecopy = true,
            IterationsOfsmoothmap = 2,
            //number of nighbors for cellular automata
            DistanceBetweenWalks = 5,
            NumberOfSteps = 250,
            NumberOfWalks = 5,
            //chance to move up, down, left or right
            RandomFillPercent = 45,
            mapGeneratorType = Content_Generation.Map_Generation.MapGeneratorType.DrunkardWalk,
            PlayerLives = 3
        };
    }
}
