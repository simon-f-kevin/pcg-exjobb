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

        public void Draw(Vector2 playerPosition)
        {
            spriteBatch.DrawString(font, NumberOfLives.ToString() , new Vector2(10, 10), Color.Black);
        }
    }
}
