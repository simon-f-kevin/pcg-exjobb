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
            DistanceBetweenEnemies = 10,

            //Goal and start position
            GoalonGround = true,

            //Cellular Automata
            UseCopy = true,
            IterationsOfsmoothmap = 2,
            NumberOfNeighborCells = 4,


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
            StartPositionForWalkers = new Microsoft.Xna.Framework.Vector2(0, 0),

            //Random placement
            RandomFillPercent = 45,


            //Which map generator to use;
            MapGeneratorType = Map_Generation.MapGeneratorType.DrunkardWalk,


            //Player lives 
            PlayerLives = 3,

            //Pitfalls
            NumberOfPitfalls = 2,
            PitfallMaxHeight = 6,
            PitfallWidth = 3
        };
    }
}
