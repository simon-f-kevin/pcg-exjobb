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

        private void TestSpawnPoint()
        {
            int X = graphics.GraphicsDevice.Viewport.Width / 2;
            int Y = graphics.GraphicsDevice.Viewport.Height / 2;
            spawnPoint = new Rectangle(new Point(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), new Point(playerTexture.Width, playerTexture.Height));

            for (int x = 0; x < X * 2; x++)
            {
                for (int y = 0; y < Y * 2; y++)
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

        private Goal GenerateFirstValidGoalPosition()
        {
            var width = Goal.Texture.Width;
            var height = Goal.Texture.Height;
            Goal.Position = new Vector2(width, Map.GetLength(1));
            for (int X = Map.GetLength(0) * width -1; X > width; --X)
                {
                for (int Y = height; Y < (Map.GetLength(1) - 1) * height; Y++)
                {
                    if (grid.IsCollidingWithCell(Goal.BoundingRectangle))
                    {
                        Goal.Position = new Vector2(X, Y);
                        Goal.BoundingRectangle = new Rectangle(new Point(X , Y ), new Point(Goal.Texture.Width, Goal.Texture.Height));
                    }
                    else { break; }
                }
            }
            return Goal;
        }

        private bool IsGoalOnGround()
        {
            bool tmp = false;
            Rectangle onePixelLower = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width));
            onePixelLower.Offset(0, 1);
            if(grid.IsCollidingWithCell(onePixelLower) && !grid.IsCollidingWithCell(new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width))))
            {
                tmp = true;
            }
           
            return tmp;
        }

        private void MoveGoalToGround()
        {
            var width = Goal.Texture.Width;
            var height = Goal.Texture.Height;
            while (!IsGoalOnGround())
            {
                Goal.Position = new Vector2(Goal.Position.X, MathHelper.Clamp((Goal.Position.Y + height), 0, graphics.GraphicsDevice.Viewport.Height - 50));
                Goal.BoundingRectangle = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Width, Goal.Texture.Height));
            }
        }

    }
}
