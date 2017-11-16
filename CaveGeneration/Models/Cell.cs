using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Cell 
    {
        public Vector2 Position { get; set; }

        public Texture2D Texture { get; set; }

        private SpriteBatch _spriteBatch { get; set; }

        public bool IsVisible { get; set; }

        private bool WalkAble { get; set; }

        public Cell(Vector2 pos, Texture2D texture, bool visible)
        {
            Position = pos;
            Texture = texture;
            IsVisible = visible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            if (IsVisible)
            {
                Rectangle destinationRect = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(Texture.Width - 1, Texture.Height - 1));
                _spriteBatch.Draw(Texture, destinationRect, color: Color.White);
            }
        }
    }
}
