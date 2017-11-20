using System;
using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveGeneration.Content_Generation.Goal_Placement
{
    public class StartAndGoalPlacer
    {
        private Goal Goal;
        private Rectangle spawnPoint;

        private Cell[,] Map;
        private Grid grid = Grid.Instance();
        private Texture2D playerTexture;
        private GraphicsDeviceManager graphics;

        public StartAndGoalPlacer(Goal goal, Texture2D playerTexture, GraphicsDeviceManager graphics)
        {
            Map = grid.Cells;
            Goal = goal;
            this.graphics = graphics;
            this.playerTexture = playerTexture;
            TestSpawnPoint();
        }

        public Rectangle GetSpawnPosition()
        {
            return spawnPoint;
        }

        public Goal GenerateReachableGoalPosition()
        {
            Goal = GenerateFirstValidGoalPosition();

            if (IsGoalOnGround())
            {
                return Goal;
            }
            else
            {
                MoveGoalToGround();
            }

            return Goal;
        }

        private Goal GenerateFirstValidGoalPosition()
        {
            Goal.Position = new Vector2(0, Map.GetLength(0));
            var width = Goal.Texture.Width;
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = Map.GetLength(0) - 1 ; x > 0; x--)
                {
                    if (grid.IsCollidingWithCell(Goal.BoundingRectangle))
                    {
                        Goal.Position = new Vector2(x, y);
                        Goal.BoundingRectangle = new Rectangle(new Point(x * width, y * width), new Point(Goal.Texture.Width, Goal.Texture.Height));
                    }
                    else { break; }
                }
            }
            return Goal;
        }

        private void TestSpawnPoint()
        {
            int X = graphics.GraphicsDevice.Viewport.Width / 2;
            int Y = graphics.GraphicsDevice.Viewport.Height / 2;
            spawnPoint = new Rectangle(new Point(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), new Point(playerTexture.Width, playerTexture.Height));

            for (int y = 0; y < X * 2; y++)
            {
                for (int x = 0; x < Y * 2; x++)
                {
                    if (grid.IsCollidingWithCell(spawnPoint))
                    {
                        spawnPoint = new Rectangle(new Point(X + x, Y + y), new Point(playerTexture.Width, playerTexture.Height));
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        private bool IsGoalOnGround()
        {
            Rectangle onePixelLower = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width));
            onePixelLower.Offset(0, 1);
            return (grid.IsCollidingWithCell(onePixelLower) && !grid.IsCollidingWithCell(new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width))));
        }

        private void MoveGoalToGround()
        {
            var width = Goal.Texture.Width;
            while (!IsGoalOnGround())
            {
                Goal.Position = new Vector2(Goal.Position.X, MathHelper.Clamp(Goal.Position.Y + 1, 0, graphics.GraphicsDevice.Viewport.Height-50));
                Goal.BoundingRectangle = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Width, Goal.Texture.Height));
            }
        }

    }
}
