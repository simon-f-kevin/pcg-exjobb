using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models.Characters
{
    public class Enemy : Character
    {
        public int enemyID;
        private bool _canJump;
        private Vector2 _spawnPoint;

        private int nBlocksLeft = 0;
        private int nBlocksRight = 0;

        public Enemy(Texture2D texture, SpriteBatch spriteBatch, bool CanJump) : base(texture, spriteBatch)
        {
            CurrentSpeed = 10;//0.5f;
            Gravity = 20;
            JumpingHeight = texture.Height * 25;
            _canJump = CanJump;

        }

        public void SetSpawnPoint(Vector2 spawn)
        {
            _spawnPoint = spawn;
            Position = spawn;
        }

        public Vector2 GetSpawnPoint()
        {
            return _spawnPoint;
        }

        public override void Update(GameTime gametime)
        {
            UpdateMovement(blocksToMove: 5);
            SimulateGravity();
            SimulateFriction();
            UpdatePosition(gametime);
            CollisionHandling(gametime);
        }

        /// <summary>
        /// This method makes the enemy move according to a set of predefined rules. 
        /// The enemy walks from its spawnpoint and then goes a certain block to one
        /// direction. It then chnages direction and moves the other way. 
        /// The enemy can jump if the CanJump boolean is true.
        /// </summary>
        private void UpdateMovement(int blocksToMove)
        {
            if(nBlocksLeft < (blocksToMove * 20))
            {
                //Move left towards player spawnpoint
                if (IsByLeftWall())
                {
                    Movement += Vector2.UnitX * (CurrentSpeed);
                }
                else
                {
                    Movement -= Vector2.UnitX * CurrentSpeed;
                }
                nBlocksLeft++;
                if (nBlocksLeft >= (blocksToMove * 20)) nBlocksRight = 0;
            }
           
            if(nBlocksLeft >= (blocksToMove * 20)) {
                //Move right
                if (IsByRightWall())
                {
                    Movement -= Vector2.UnitX * (CurrentSpeed);
                }
                Movement += Vector2.UnitX * CurrentSpeed;
                nBlocksRight++;
                if (nBlocksRight == (blocksToMove * 20)) nBlocksLeft = 0;
            }

            if (_canJump)
            {
                if (IsByLeftWall())
                {
                    Movement = -Vector2.UnitY * (JumpingHeight * 1.1f);
                    Movement += Vector2.UnitX * (CurrentSpeed * 7.5f);
                }
                if (IsByRightWall())
                {
                    Movement = -Vector2.UnitY * (JumpingHeight * 1.1f);
                    Movement -= Vector2.UnitX * (CurrentSpeed * 7.5f);
                }
            }

        }

        protected override void SimulateFriction()
        {
            if (IsOnGround())
            {
                Movement -= Movement * Vector2.One * .08f;
            }
            else
            {
                Movement -= Movement * Vector2.One * .01f;
            }
        }
    }
}
