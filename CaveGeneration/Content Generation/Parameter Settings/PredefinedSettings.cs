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
            //Enemy placement
            EnemyCount = 3,
            //distance between enemeies



            //Goal and start position  NOT IMPLEMENTED
            GoalonGround = true,

            //Cellular Automata
            usecopy = true,
            IterationsOfsmoothmap = 2,
            //number of neighbors for cellular automata


            //Drunkard Walk
            DistanceBetweenWalks = 5,
            NumberOfSteps = 250,
            NumberOfWalks = 5,
            DrunkardDirections = new Map_Generation.DrunkardWalk.Directions()
            {
                Up = 20,
                Down = 20,
                Left = 20,
                Right = 40
            },

            //Random placement
            RandomFillPercent = 45,


            //Which map generator to use;
            mapGeneratorType = Content_Generation.Map_Generation.MapGeneratorType.DrunkardWalk,


            //Player lives
            PlayerLives = 3
        };
    }
}
