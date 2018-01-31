using CaveGeneration.Content_Generation.Map_Generation;
using Microsoft.Xna.Framework;
using static CaveGeneration.Content_Generation.Map_Generation.DrunkardWalk;

namespace CaveGeneration.Content_Generation.Parameter_Settings
{
    public class Settings
    {
        public string Seed { get; set; }
        public bool IncrementDifficulty { get; set; }
        public int IncrementChance { get; set; }

        public int EnemyCount { get; set; }

        public int StaticEnemyChance { get; set; }

        public bool EnemiesCanJump { get; set; }

        public int DistanceBetweenEnemies { get; set; }

        public bool GoalonGround { get; set; }

        public int IterationsOfsmoothmap { get; set; }

        public bool UseCopy { get; set; }

        public int NumberOfNeighborCells { get; set; }

        public MapGeneratorType MapGeneratorType { get; set; }

        public int NumberOfWalks { get; set; }

        public int NumberOfSteps { get; set; }

        public int DistanceBetweenWalks { get; set; }

        public Vector2 StartPositionForWalkers { get; set; }

        public int RandomFillPercent { get; set; }

        public int PlayerLives { get; set; }

        public Directions DrunkardDirections;

        public int NumberOfPitfalls { get; set; }

        public int PitfallWidth { get; set; }

        public int PitfallMaxHeight { get; set; }

    }
}
