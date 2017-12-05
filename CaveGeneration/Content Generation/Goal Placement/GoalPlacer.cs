using System;
using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CaveGeneration.Content_Generation.Astar;
using System.Collections.Generic;

namespace CaveGeneration.Content_Generation.Goal_Placement
{
    public class StartAndGoalPlacer
    {
        private Goal Goal;
        private Rectangle spawnPoint;

        private Cell[,] Map;
        private Grid grid = Grid.Instance();
        private Texture2D playerTexture;
        private Character player;
        private GraphicsDeviceManager graphics;

        public StartAndGoalPlacer(Goal goal, Texture2D texture, GraphicsDeviceManager graphics)
        {
            Map = grid.Cells;
            Goal = goal;
            this.graphics = graphics;
            playerTexture = texture;
            TestSpawnPoint();
        }

        public void SetPlayer(Character player)
        {
            this.player = player;
        }

        public Rectangle GetSpawnPosition()
        {
            return spawnPoint;
        }

        public Goal GenerateReachableGoalPosition()
        {
            Goal = GenerateFirstValidGoalPosition();
            LinkedList<Cell> path = null;
            while(path == null)
            {
                path = TestIfMapIsSolveable();
                if (IsGoalOnGround() && path != null)
                {
                    return Goal;
                }
                else if(IsGoalOnGround() && path == null)
                {
                    MoveGoalToLeft();
                }
                else
                {
                    MoveGoalToGround();
                   
                }
            }
            return Goal;
        }

        private void TestSpawnPoint()
        {
            int X = Map.GetLength(0) * 20;
            int Y = Map.GetLength(1) / 2;
            spawnPoint = new Rectangle(new Point(0, 0), new Point(playerTexture.Width, playerTexture.Height));

            for (int x = 1; x < X; x++)
            {
                for (int y = 0; y < Y - 1; y++)
                {
                    if (grid.IsCollidingWithCell(spawnPoint))
                    {
                        spawnPoint = new Rectangle(new Point(x * playerTexture.Width, y * playerTexture.Height), new Point(playerTexture.Width, playerTexture.Height));
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
            for (int X = (Map.GetLength(0) - 1) * width ; X > width; X--)
            {
                for (int Y = height; Y < (Map.GetLength(1) - 1) * height ; Y++)
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

        private void MoveGoalToLeft()
        {

        }

        private LinkedList<Cell> TestIfMapIsSolveable()
        {

            SpatialAStar<Cell, Object> aStar = new SpatialAStar<Cell, Object>(grid.Cells);
            LinkedList<Cell> path = aStar.Search(new Point((int)player.Position.X / 20, (int)player.Position.Y / 20),
               new Point((int)Goal.Position.X / 20, (int)Goal.Position.Y / 20), null);
            return path;
        }
    }
}
