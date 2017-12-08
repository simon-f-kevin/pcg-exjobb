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
        public float MaxSpeed { get; set; }
        public float JumpingHeight { get; set; }
        public float Gravity { get; set; }

        public bool Alive { get; set; }

        private Grid grid = Grid.Instance();
        private SpriteBatch SpriteBatch;
        private Vector2 oldPosition;

        private int frameCount = 0;
        bool groundJump;

        List<Action> oldInput = null;

        public Character(Texture2D texture, Vector2 position, SpriteBatch spiteBatch)
        {
            Position = position;
            Texture = texture;
            SpriteBatch = spiteBatch;
            MaxSpeed = 2;
            JumpingHeight = texture.Height * 1.5f;
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
            List<Action> actions = Input.GetInput();
            if(groundJump)
            frameCount++;
            if (frameCount > 10)
            {
                groundJump = false;
                frameCount = 0;
            }

            if (IsByWall() && !IsOnGround())
            {
                if (actions.Contains(Action.MoveUp) && (actions.Contains(Action.MoveLeft) && !groundJump))
                {
                    if (!oldInput.Contains(Action.MoveUp)) { 
                        Movement = -Vector2.UnitY * (JumpingHeight * 1.1f);
                        Movement += Vector2.UnitX * (MaxSpeed * 7.5f);
                        groundJump = true;
                        return;
                    }
                }
                else if (actions.Contains(Action.MoveUp) && (actions.Contains(Action.MoveRight) && !groundJump))
                {
                    if (!oldInput.Contains(Action.MoveUp)) {
                        Movement = -Vector2.UnitY * (JumpingHeight * 1.1f);
                        Movement -= Vector2.UnitX * (MaxSpeed * 7.5f);
                        groundJump = true;
                        return;
                    }
                }
            }
            else if (!IsOnGround())
            {
                if (actions.Contains(Action.MoveLeft))
                    { Movement -= Vector2.UnitX * (MaxSpeed * 1.1f); }
                if (actions.Contains(Action.MoveRight))
                    { Movement += Vector2.UnitX * (MaxSpeed * 1.1f); }
            }

            if (IsOnGround() && (actions.Contains(Action.MoveUp)))
            {
                groundJump = true;
                Movement -= Vector2.UnitY * JumpingHeight;
            }

            if (IsOnGround() && actions.Contains(Action.MoveLeft))
                { Movement -= Vector2.UnitX * MaxSpeed; }
            if (IsOnGround() && actions.Contains(Action.MoveRight))
                { Movement += Vector2.UnitX * MaxSpeed; }

            oldInput = actions;

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
            if (IsOnGround())
            {
                KeyboardState kbState = Keyboard.GetState();
                if (!kbState.IsKeyDown(Keys.Space) && !kbState.IsKeyDown(Keys.Up))
                {
                    Movement -= Movement * Vector2.One * .08f;
                }
            }
            else { Movement -= Movement * Vector2.One * .01f; }
        }

        private bool IsOnGround()
        {
            Rectangle onePixelLower = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            onePixelLower.Offset(0, 1);
            return grid.IsCollidingWithCell(onePixelLower);
        }

        private bool IsByWall()
        {
            Rectangle boundingCharacter = new Rectangle((int)Position.X - 1, (int)Position.Y, Texture.Width + 2, Texture.Height);
            return grid.IsCollidingWithCell(boundingCharacter);
        }

    }
}
