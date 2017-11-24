using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Character
    {
        public Vector2 Movement { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public float MaxSpeed { get; set; }
        public float JumpingHeight { get; set; }
        public float Gravity { get; set; }

        public bool Alive { get; set; }

        private Grid grid = Grid.Instance();

        private Vector2 oldPosition;

        public Character(Texture2D texture, Vector2 position, SpriteBatch spiteBatch)
        {
            Position = position;
            Texture = texture;
            SpriteBatch = spiteBatch;
            MaxSpeed = 2;
            JumpingHeight = texture.Height;
            Gravity = 2;
            Alive = true;
        }

        public void Update(GameTime gametime)
        {
            GetInputAndUpdateMovement();
            SimulateGravity();
            SimulateFriction();
            UpdatePosition(gametime);
            CollisionHandling(gametime);
           
        }
        public void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

        private void UpdatePosition(GameTime gametime)
        {
            oldPosition = Position;
            Position += Movement * (float)gametime.ElapsedGameTime.TotalMilliseconds / 60;
            Position = grid.WhereCanIGetTo(oldPosition, Position, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height));
        }

        private void GetInputAndUpdateMovement()
        {
            KeyboardState kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Left)) { Movement -= Vector2.UnitX * MaxSpeed; }
            if (kbState.IsKeyDown(Keys.Right)) { Movement += Vector2.UnitX * MaxSpeed; }
            if (kbState.IsKeyDown(Keys.Space) && IsOnGround() || kbState.IsKeyDown(Keys.Up) && IsOnGround()) { Movement -= Vector2.UnitY * JumpingHeight; }
        }

        private void CollisionHandling(GameTime gametime)
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }

        private void SimulateGravity()
        {
            Movement += Vector2.UnitY * Gravity;
        }

        private void SimulateFriction()
        {
            if (IsOnGround()) { Movement -= Movement * Vector2.One * .08f; }
            else { Movement -= Movement * Vector2.One * .02f; }
        }

        private bool IsOnGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            onePixelLower.Offset(0, 1);
            return grid.IsCollidingWithCell(onePixelLower);
        }

    }
}
