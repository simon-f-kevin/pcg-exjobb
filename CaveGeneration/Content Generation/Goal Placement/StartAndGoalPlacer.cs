using System;
using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CaveGeneration.Content_Generation.Astar;
using System.Collections.Generic;
using CaveGeneration.Models.Characters;
using CaveGeneration.Content_Generation.Parameter_Settings;

namespace CaveGeneration.Content_Generation.Goal_Placement
{
    public class StartAndGoalPlacer
    {
        private Goal Goal;
        private Rectangle spawnPoint;

        private Cell[,] Map;
        private Grid grid = Grid.Instance();
        private Texture2D playerTexture;
        private Player player;
        private GraphicsDeviceManager graphics;

        private Settings settings;


        public StartAndGoalPlacer(Goal goal, Texture2D texture, GraphicsDeviceManager graphics, Settings settings)
        {
            Map = grid.Cells;
            Goal = goal;
            this.graphics = graphics;
            this.settings = settings;
            playerTexture = texture;
            TestSpawnPoint();
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public Rectangle GetSpawnPosition()
        {
            return spawnPoint;
        }

        public Goal GenerateReachableGoalPosition()
        {
            int nLeftMoves = 0;
            Goal = GenerateFirstValidGoalPosition();
            LinkedList<Cell> path = null;
            while (path == null)
            {
                if (nLeftMoves == 10)
                {
                    throw new NotSolveableException("Not solveable");
                    //return Goal;
                }
                path = TestIfMapIsSolveable();
                if (IsGoalReachable() && path != null)
                {
                    return Goal;
                }
                else if (IsGoalReachable() && path == null)
                {
                    MoveGoalToLeft();
                    nLeftMoves++;
                }
                else
                {
                    if(path == null)
                    {
                        MoveGoalToLeft();
                        nLeftMoves++;
                    }
                    if(settings.GoalonGround == true)
                    {
                        MoveGoalToGround();
                    }
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
                    if (grid.IsCollidingWithCell(spawnPoint) || !SpawnAboveGround(x,y))
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

        private bool SpawnAboveGround(int x, int y)
        {
            for (int i = 0; i < Map.GetLength(1) - 1; i++)
            {
                if (Map[x, y+i].IsVisible)
                {
                    return true;
                }
            }
            return false;
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

        private bool IsGoalReachable()
        {
            bool tmp = false;
            Rectangle oneBlockLower = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width));
            oneBlockLower.Offset(0, 1);
            if(grid.IsCollidingWithCell(oneBlockLower) && !grid.IsCollidingWithCell(new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Height, Goal.Texture.Width))))
            {
                tmp = true;
            }
           
            return tmp;
        }

        private void MoveGoalToGround()
        {
            var width = Goal.Texture.Width;
            var height = Goal.Texture.Height;
            while (!IsGoalReachable() && Goal.Position.Y < ((grid.HeightInBlocks - 2) * 20))
            {
                Goal.Position = new Vector2(Goal.Position.X, MathHelper.Clamp((Goal.Position.Y + height), 0, graphics.GraphicsDevice.Viewport.Height - 50));
                Goal.BoundingRectangle = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Width, Goal.Texture.Height));
            }
        }

        private void MoveGoalToLeft()
        {
            var width = Goal.Texture.Width;
            var height = Goal.Texture.Height;

            Goal.Position = new Vector2(MathHelper.Clamp(Goal.Position.X - width, width, Goal.Position.X), Goal.Position.Y);
            Goal.BoundingRectangle = new Rectangle(new Point((int)Goal.Position.X, (int)Goal.Position.Y), new Point(Goal.Texture.Width, Goal.Texture.Height));
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
