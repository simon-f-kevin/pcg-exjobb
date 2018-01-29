using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Goal
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Rectangle BoundingRectangle {get; set;}

        public Goal(Vector2 pos, Texture2D texture, SpriteBatch spriteBatch)
        {
            Position = pos;
            Texture = texture;
            SpriteBatch = spriteBatch;
            BoundingRectangle = new Rectangle(new Point((int)pos.X, (int)pos.Y), new Point(Texture.Width, Texture.Height));
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, BoundingRectangle, Color.White);
        }

    }
}
