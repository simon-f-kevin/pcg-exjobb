using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveGeneration.Models.Characters
{
    public abstract class Character
    {
        public Vector2 Movement { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float MaxSpeed { get; set; }
        public float JumpingHeight { get; set; }
        public float Gravity { get; set; }
        

        protected Grid grid = Grid.Instance();
        protected SpriteBatch SpriteBatch;
        protected Vector2 oldPosition;

        public abstract void Update(GameTime gametime);

        public void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        protected void SimulateGravity()
        {
            Movement += Vector2.UnitY * Gravity;
        }

        protected abstract void SimulateFriction();

        protected void UpdatePosition(GameTime gametime)
        {
            oldPosition = Position;
            Position += Movement * (float)gametime.ElapsedGameTime.TotalMilliseconds / 60;
            Position = grid.WhereCanIGetTo(oldPosition, Position, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height));
        }

        protected void CollisionHandling(GameTime gametime)
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }

        protected bool IsOnGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            onePixelLower.Offset(0, 1);
            return grid.IsCollidingWithCell(onePixelLower);
        }

        protected bool IsByLeftWall()
        {
            Rectangle boundingCharacter = new Rectangle((int)Position.X - 1, (int)Position.Y, Texture.Width + 1, Texture.Height);
            return grid.IsCollidingWithCell(boundingCharacter);
        }

        protected bool IsByRightWall()
        {
            Rectangle boundingCharacter = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width + 1, Texture.Height);
            return grid.IsCollidingWithCell(boundingCharacter);
        }
    }
}
