using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Grid
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Cell[,] Cells { get; set; }
    }
}
