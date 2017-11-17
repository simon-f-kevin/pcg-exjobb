using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Grid
    {
        public int Columns { get; set; }
        public int Rows { get; set; }

        public Cell[,] Cells { get; set; }

        public Texture2D CellTexture { get; set; }

        private SpriteBatch _spriteBatch { get; set; }

        private DrunkenCells dw;
        private CellularAutomata cg;

        private static Grid _instance;

        public static Grid CreateNewGrid(int x, int y, SpriteBatch sb, Texture2D texture, string seed)
        {
            if(_instance == null)
            {
                _instance = new Grid(x, y, sb, texture, seed);
                return _instance;
            }
            return _instance;
        }

        public static Grid Instance()
        {
            return _instance;
        }

        private Grid(int x, int y, SpriteBatch sb, Texture2D texture, string seed)
        {
            Columns = x;
            Rows = y;
            _spriteBatch = sb;
            CellTexture = texture;
            cg = new CellularAutomata(Columns, Rows, 45);
            Init(seed);
        }

        private void Init(string seed)
        {
            Cells = new Cell[Columns, Rows];
            cg.Start(seed);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 pos = new Vector2(x * CellTexture.Width, y * CellTexture.Height);
                    Cells[x, y] = new Cell(pos, CellTexture, false);
                }
            }
            int[,] map = cg.GetMap();

            int col = map.GetLength(0);
            int row = map.GetLength(1);
            for (int x = 0; x < col; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    if(map[x,y] == 1)
                    {
                        Cells[x, y].IsVisible = true;
                    }
                }
            }

            Cells[Columns / 2, Rows / 2].IsVisible = false;
        }

        public bool IsCollidingWithCell(Rectangle rectangleToCheck)
        {
            foreach (var cell in Cells)
            {
                var boundingRectangle = new Rectangle((int)cell.Position.X, (int)cell.Position.Y, cell.Texture.Width, cell.Texture.Height);
                if (cell.IsVisible && boundingRectangle.Intersects(rectangleToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw()
        {
            foreach(var cell in Cells)
            {
                cell.Draw(_spriteBatch);
            }
        }
    }
}
