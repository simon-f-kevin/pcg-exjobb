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

        private CaveGenerator cg;



        public Grid(int x, int y, SpriteBatch sb, Texture2D texture, string seed)
        {
            Columns = x;
            Rows = y;
            _spriteBatch = sb;
            CellTexture = texture;
            cg = new CaveGenerator(Columns, Rows, 50);
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
