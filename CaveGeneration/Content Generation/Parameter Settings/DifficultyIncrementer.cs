using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Parameter_Settings
{
    static class DifficultyIncrementer
    {
        public static void Increment(Settings settings, int seed)
        {
            Random rand = new Random(seed);

            if(rand.Next(settings.IncrementChance) == 1)
                settings.NumberOfPitfalls++;
            if (rand.Next(settings.IncrementChance) == 1)
                settings.PitfallWidth++;
            if (rand.Next(settings.IncrementChance) == 1)
                settings.PitfallMaxHeight++;
            if (rand.Next(settings.IncrementChance) == 1)
                settings.EnemyCount++;
        }

        public static void Increment(Settings settings, string seed)
        {
            Increment(settings, seed.GetHashCode());
        }
    }
}
