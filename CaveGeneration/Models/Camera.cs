using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Camera
    {
        public Matrix transform;
        Viewport viewport;
        Vector2 center;


        public Camera(Viewport newView)
        {
            viewport = newView;
        }

        public void Update(GameTime gameTime, Game1 player)
        {
            center = new Vector2(player.playerPosition.X + (player.playerRectangle.Width / 2) - (viewport.Width / 2), player.playerPosition.Y + (player.playerRectangle.Height / 2 - (viewport.Height/2)));
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                        Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
