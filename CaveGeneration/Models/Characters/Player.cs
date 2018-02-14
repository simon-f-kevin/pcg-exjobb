using CaveGeneration.Content_Generation.Parameter_Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CaveGeneration.Models.Characters
{
    public class Player : Character
    {
        public bool Alive { get; set; }

        private int frameCount = 0;
        bool groundJump;

        List<Action> oldInput = null;

        int hp;

        public bool hurt = false;

        int hurtmaxframes = 60;
        int hurtframes = 0;

        private float superJumpHeight;
        private float regularJumpHeight;
        private float slowJumpHeight;
        private float slowSpeed;
        private float defaultSpeed;


        public Player(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, Settings settings) : base(texture, spriteBatch)
        {
            Position = position;
            CurrentSpeed = 40;
            JumpingHeight = texture.Height * 27;
            regularJumpHeight = JumpingHeight;
            superJumpHeight = JumpingHeight * 5;
            slowJumpHeight = JumpingHeight * 0.6f;
            slowSpeed = CurrentSpeed / 2;
            defaultSpeed = CurrentSpeed;
            Gravity = 30;
            hp = settings.PlayerLives;
            Alive = true;
        }

        public new void Draw(GameTime gameTime)
        {
            if (hurt)
            {
                if(hurtframes < hurtmaxframes)
                {
                    hurtframes++;
                    SpriteBatch.Draw(Texture, Position, Color.Red);
                }
                else
                {
                    hurtframes = 0;
                    hurt = false;
                    SpriteBatch.Draw(Texture, Position, Color.White);
                }
            }
            else
            {
                SpriteBatch.Draw(Texture, Position, Color.White);
            }
        }

        public void DealDamage()
        {
            if(hp > 0)
            {
                hp--;
            }
            if (hp <= 0)
            {
                Alive = false;
            }
            else
            {
                hurt = true;
            }
        }

        public bool IsAlive()
        {
            if (hp <= 0)
            {
                Alive = false;
            }
            else Alive = true;

            return Alive;
        }

        public int GetHp()
        {
            return hp;
        }

        public override void Update(GameTime gametime)
        {
            GetInputAndUpdateMovement();
            SimulateGravity();
            SimulateFriction();
            UpdatePosition(gametime);
            CollisionHandling(gametime);
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

            bool leftWall = IsByLeftWall();
            bool rightWall = IsByRightWall();

            if (actions.Contains(Action.Suicide))
            {
                hurt = true;
                hp = 0;
                Alive = false;
            }

            if (actions.Contains(Action.SuperJump) && actions.Contains(Action.MoveUp) && IsOnGround() && hp > 1)
            {
                JumpingHeight = superJumpHeight;
                DealDamage();
            }
            else if (actions.Contains(Action.Slow))
            {
                JumpingHeight = slowJumpHeight;
                CurrentSpeed = slowSpeed;
            }
            else
            {
                JumpingHeight = regularJumpHeight;
                CurrentSpeed = defaultSpeed;
            }


            if ((leftWall || rightWall) && !IsOnGround())
            {
                if (actions.Contains(Action.MoveUp) && (actions.Contains(Action.MoveLeft) && leftWall && !groundJump))
                {
                    if (!oldInput.Contains(Action.MoveUp)) {
                        Movement = -Vector2.UnitY * (JumpingHeight); 
                        Movement += Vector2.UnitX * (MaximumSpeed); 
                        groundJump = true;
                        return;
                    }
                }
                else if (actions.Contains(Action.MoveUp) && (actions.Contains(Action.MoveRight) && rightWall && !groundJump))
                {
                    if (!oldInput.Contains(Action.MoveUp)) {
                        Movement = -Vector2.UnitY * (JumpingHeight); 
                        Movement -= Vector2.UnitX * (MaximumSpeed);
                        groundJump = true;
                        return;
                    }
                }
                else if (!IsOnGround())
                {
                    if (actions.Contains(Action.MoveLeft))
                    { Movement -= Vector2.UnitX * (CurrentSpeed); }
                    if (actions.Contains(Action.MoveRight))
                    { Movement += Vector2.UnitX * (CurrentSpeed); }
                }
            }
            else if (!IsOnGround())
            {
                if (actions.Contains(Action.MoveLeft))
                { Movement -= Vector2.UnitX * (CurrentSpeed); } 
                if (actions.Contains(Action.MoveRight))
                { Movement += Vector2.UnitX * (CurrentSpeed); }
            }

            if (IsOnGround() && (actions.Contains(Action.MoveUp)))
            {
                groundJump = true;
                Movement -= Vector2.UnitY * JumpingHeight;
            }

            if (IsOnGround() && actions.Contains(Action.MoveLeft))
                { Movement -= Vector2.UnitX * CurrentSpeed; }
            if (IsOnGround() && actions.Contains(Action.MoveRight))
                { Movement += Vector2.UnitX * CurrentSpeed; }

            oldInput = actions;

        }

        protected override void SimulateFriction()
        {
            KeyboardState kbState = Keyboard.GetState();
            if (IsOnGround())
            {   
                if (!kbState.IsKeyDown(Keys.Space) && !kbState.IsKeyDown(Keys.Up))
                {
                    Movement -= Movement * .08f;
                }
            }
            else {
                Movement -= Movement * Vector2.One * .01f;
            }
        }

    }
}
