using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Cell
    {
        public Cell(int width, int height, bool visible)
        {
            Width = width;
            Height = height;
            Transparent = visible;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool Transparent { get; set; }
    }
}
