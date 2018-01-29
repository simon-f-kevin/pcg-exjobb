using CaveGeneration.Models.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public void Update(GameTime gameTime, Player player)
        {
            center = new Vector2(player.Position.X + (player.BoundingRectangle.Width / 2) - (viewport.Width / 2), player.Position.Y + (player.BoundingRectangle.Height / 2 - (viewport.Height/2)));
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                        Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
