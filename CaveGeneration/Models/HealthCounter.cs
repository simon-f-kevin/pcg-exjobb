using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class HealthCounter
    {
        int NumberOfLives;
        SpriteBatch spriteBatch;
        SpriteFont font;

        public HealthCounter(SpriteBatch spriteBatch, SpriteFont font)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;

        }

        public void Update(int hp)
        {
            NumberOfLives = hp;
        }

        private void ControlDrawingPosition(Vector2 playerPosition)
        {
            Vector2 startpos = playerPosition;
        }

        private Vector2 DrawingPosition(Vector2 playerPosition)
        {
            int x = 10;
            int y = -50;
            if(playerPosition.Y > 200)
            {
                y = (int)playerPosition.Y -250;
            }
            if (playerPosition.X > 700) 
            {
                x = (int)playerPosition.X - 700;

            }
            return new Vector2(x, y);
        }

        public void Draw(Vector2 playerPosition)
        {
            spriteBatch.DrawString(font, NumberOfLives.ToString() , DrawingPosition(playerPosition), Color.Black);
        }
    }
}
