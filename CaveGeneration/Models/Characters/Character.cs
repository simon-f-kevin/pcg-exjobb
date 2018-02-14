using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CaveGeneration.Models.Characters
{
    public abstract class Character 
    {
        public Vector2 Movement { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float CurrentSpeed { get; set; }
        public float MaximumSpeed { get; set; }
        public float JumpingHeight { get; set; }
        public float Gravity { get; set; }

        public Rectangle BoundingRectangle;

        protected Grid grid = Grid.Instance();
        protected SpriteBatch SpriteBatch;
        protected Vector2 oldPosition;

        public Character(Texture2D texture, SpriteBatch spriteBatch)
        {
            Texture = texture;
            SpriteBatch = spriteBatch;
            MaximumSpeed = 200;
            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public abstract void Update(GameTime gametime);

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        public bool IsInsideStage(Rectangle stage)
        {
            if (!BoundingRectangle.Intersects(stage)) return false;
            else return true;
        }

        protected void SimulateGravity()
        {
            Movement += Vector2.UnitY * Gravity;
        }

        protected abstract void SimulateFriction();

        protected void UpdatePosition(GameTime gametime)
        {
            float dT = (float)gametime.ElapsedGameTime.TotalSeconds;
            oldPosition = Position;
            ConstrainMovementToMaxSpeed();
            Position += Movement * dT;
            Position = grid.WhereCanIGetTo(oldPosition, Position, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height));
            BoundingRectangle.X = (int)Position.X;
            BoundingRectangle.Y = (int)Position.Y;
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

        private void ConstrainMovementToMaxSpeed()
        {
            if (Math.Abs(Math.Abs(Movement.X) - MaximumSpeed) > MaximumSpeed)
            {
                if (Movement.X > MaximumSpeed)
                {
                    Movement = new Vector2(MaximumSpeed, Movement.Y);
                }
                if (Movement.X < 0 && Movement.X < - MaximumSpeed)
                {
                    Movement = new Vector2(-MaximumSpeed, Movement.Y);
                }
            }
        }

    }
}
