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

        private Vector2 oldPosition;

        public Character(Texture2D texture, Vector2 position, SpriteBatch spiteBatch)
        {
            Position = position;
            Texture = texture;
            SpriteBatch = spiteBatch;
            MaxSpeed = .9f;
            JumpingHeight = texture.Height;
            Gravity = .65f;
        }

        public void Update(GameTime gametime)
        {
            UpdatePosition();
            SimulateGravity();
            Collision();
        }

        private void SimulateGravity()
        {
            Position += Vector2.UnitY * Gravity;
        }

        private void Collision()
        {
            Vector2 lastMovement = Position - oldPosition;
            if (lastMovement.X == 0) { Movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { Movement *= Vector2.UnitX; }
        }

        private void UpdatePosition()
        {
            KeyboardState kbState = Keyboard.GetState();

            if(kbState.IsKeyDown(Keys.Left)) { Position -= Vector2.UnitX * MaxSpeed; }
            if(kbState.IsKeyDown(Keys.Right)) { Position += Vector2.UnitX * MaxSpeed; }
            if(kbState.IsKeyDown(Keys.Space) || kbState.IsKeyDown(Keys.Up) && IsOnGround()) { Movement -= Vector2.UnitY * JumpingHeight; }
        }

        private bool IsOnGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            onePixelLower.Offset(0, 1);
            return !Grid.Instance().IsCollidingWithCell(onePixelLower);
        }

        public void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);
        }

    }
}
