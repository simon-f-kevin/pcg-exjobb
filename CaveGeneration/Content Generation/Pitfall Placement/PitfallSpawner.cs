using CaveGeneration.Content_Generation.Parameter_Settings;
using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Pitfall_Placement
{
    class PitfallSpawner
    {
        private Cell[,] Map;
        private Grid grid;
        private Settings settings;

        private int collumns;
        private int rows;

        private int NumberOfPitfalls;
        private int PitfallWidth;
        private int PitfallMaxHeight;

        




        public PitfallSpawner(Settings settings)
        {
            grid = Grid.Instance();
            this.Map = grid.Cells;
            this.settings = settings;

            collumns = Map.GetLength(0);
            rows = Map.GetLength(1);

            this.NumberOfPitfalls = settings.NumberOfPitfalls;
            this.PitfallMaxHeight = settings.PitfallMaxHeight;
            this.PitfallWidth = settings.PitfallWidth;

            

        }


        public void GeneratePitfalls()
        {
            var PositionList = FindPitfallPositions();
            Random rand = new Random();
            for(int i = 0; i < NumberOfPitfalls; i++)
            {
                int pos = rand.Next(PositionList.Count);
                var PitfallPosition = PositionList.ElementAt(pos);
                PlacePitfall((int)PitfallPosition.X, (int)PitfallPosition.Y);
                PositionList.Remove(PitfallPosition);
            }
        }

        private LinkedList<Vector2> FindPitfallPositions()
        {
            LinkedList<Vector2> positionList = new LinkedList<Vector2>();

            for (int y = rows - 1; y >= rows - PitfallMaxHeight; y--)
            {
                for (int x = 0; x < collumns; x++)
                {
                    if (ValidPosition(x, y))
                    {
                        positionList.AddLast(new Vector2(x, y));
                    }
                }
            }
            return positionList;
        }

        private bool ValidPosition(int xPosition, int yPosition)
        {
            for(int x = xPosition; x < xPosition+PitfallWidth; x++)
            {
                if (Map[x, yPosition].IsVisible == true)
                {
                    return false;
                }
            }
            return true;
        }

        private void PlacePitfall(int xPosition, int yPosition)
        {
            for (int x = xPosition; x < xPosition + PitfallWidth; x++)
            {
                for(int y = yPosition; y < rows; y++)
                {
                    Map[x, y].IsVisible = false;
                }
            }
        }
    }
}
