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
            //Set seed. Leave empty if you want a random map
            Seed = "",

            //Increment Difficulty, higher chance is harder
            IncrementChance = 66,
            IncrementDifficulty = true,

            //Enemy placement
            EnemyCount = 2,
            DistanceBetweenEnemies = 10,
            EnemiesCanJump = true,
            StaticEnemyChance = 30,

            //Goal and start position
            GoalonGround = true,

            //Cellular Automata
            UseCopy = true,
            IterationsOfsmoothmap = 2,
            NumberOfNeighborCells = 4,


            //Which map generator to use;
            MapGeneratorType = Map_Generation.MapGeneratorType.DrunkardWalk,

            //Drunkard Walk settings, ignore if MapGeneratorType is Random Placement
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

            //Random placement settings, ignore if MapGeneratorType is Drunkard Walk
            RandomFillPercent = 45,

            //Player lives 
            PlayerLives = 3,

            //Pitfalls
            NumberOfPitfalls = 0,
            PitfallMaxHeight = 6,
            PitfallWidth = 2
        };

        public static Settings settings2 = new Settings()
        {
            //Set seed. Leave empty if you want a random map
            Seed = "",

            //Increment Difficulty, higher chance is harder
            IncrementChance = 50,
            IncrementDifficulty = true,

            //Enemy placement
            EnemyCount = 4,
            DistanceBetweenEnemies = 10,
            EnemiesCanJump = true,
            StaticEnemyChance = 60,

            //Goal and start position
            GoalonGround = true,

            //Cellular Automata
            UseCopy = true,
            IterationsOfsmoothmap = 2,
            NumberOfNeighborCells = 4,


            //Which map generator to use;
            MapGeneratorType = Map_Generation.MapGeneratorType.RandomPlacement,

            //Drunkard Walk settings, ignore if MapGeneratorType is Random Placement
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

            //Random placement settings, ignore if MapGeneratorType is Drunkard Walk
            RandomFillPercent = 45,

            //Player lives 
            PlayerLives = 3,

            //Pitfalls
            NumberOfPitfalls = 0,
            PitfallMaxHeight = 6,
            PitfallWidth = 2
        };
        public static Settings settings3 = new Settings()
        {
            //Set seed. Leave empty if you want a random map
            Seed = "",

            //Increment Difficulty, higher chance is harder
            IncrementChance = 75,
            IncrementDifficulty = true,

            //Enemy placement
            EnemyCount = 2,
            DistanceBetweenEnemies = 10,
            EnemiesCanJump = true,
            StaticEnemyChance = 100,

            //Goal and start position
            GoalonGround = true,

            //Cellular Automata
            UseCopy = true,
            IterationsOfsmoothmap = 2,
            NumberOfNeighborCells = 4,


            //Which map generator to use;
            MapGeneratorType = Map_Generation.MapGeneratorType.DrunkardWalk,

            //Drunkard Walk settings, ignore if MapGeneratorType is Random Placement
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

            //Random placement settings, ignore if MapGeneratorType is Drunkard Walk
            RandomFillPercent = 45,

            //Player lives 
            PlayerLives = 3,

            //Pitfalls
            NumberOfPitfalls = 2,
            PitfallMaxHeight = 7,
            PitfallWidth = 2
        };
    }
}
