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

            if(rand.Next(100) <= settings.IncrementChance)
                settings.NumberOfPitfalls++;
            if (rand.Next(100) <= settings.IncrementChance)
                while(settings.PitfallWidth < 3) settings.PitfallWidth++;
            if (rand.Next(100) <= settings.IncrementChance)
                settings.PitfallMaxHeight++;
            if (rand.Next(100) <= settings.IncrementChance)
                settings.EnemyCount++;
        }

        public static void Increment(Settings settings, string seed)
        {
            Increment(settings, seed.GetHashCode());
        }
    }
}
