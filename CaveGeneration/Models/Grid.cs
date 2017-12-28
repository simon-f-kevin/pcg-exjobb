using CaveGeneration.Content_Generation.Map_Cleanup;
using CaveGeneration.Content_Generation.Map_Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    public class Grid
    {
        public int WidthInBlocks { get; set; }
        public int HeightInBlocks { get; set; }

        public Cell[,] Cells { get; set; }

        public Texture2D CellTexture { get; set; }

        private SpriteBatch _spriteBatch { get; set; }

        private MapGenerator mapGenerator;
        private MapCleanup mapCleaner;

        private static Grid _instance;

        public static Grid CreateNewGrid(int gridWidth, int gridHeight, SpriteBatch sb, Texture2D texture, string seed, int iterationsOfSmoothmap, bool useCopyOfMap)
        {
            if(_instance == null)
            {
                _instance = new Grid(gridWidth, gridHeight, sb, texture, seed, iterationsOfSmoothmap, useCopyOfMap);
                return _instance;
            }
            return _instance;
        }

        public static Grid Instance()
        {
            return _instance;
        }

        public static void ClearInstance()
        {
            _instance = null;
        }

        public void Draw()
        {
            foreach (var cell in Cells)
            {
                cell.Draw(_spriteBatch);
            }
        }
        public bool IsCollidingWithCell(Rectangle rectangleToCheck)
        {
            foreach (var cell in Cells)
            {
                var boundingRectangle = new Rectangle((int)cell.Position.X, (int)cell.Position.Y, cell.Texture.Width, cell.Texture.Height);
                if (cell.IsVisible && boundingRectangle.Intersects(rectangleToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsGround(int x, int y)
        {
            if (Cells[x, y].IsVisible) return true;
            else return false;
        }

        public int[,] GetCellsAsIntArray()
        {
            return mapGenerator.GetMap();
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = new Rectangle((int)positionToTry.X, (int)positionToTry.Y, boundingRectangle.Width, boundingRectangle.Height);
                if (!IsCollidingWithCell(newBoundary))
                {
                    move.FurthestAvailableLocationSoFar = positionToTry;
                }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Grid(int x, int y, SpriteBatch sb, Texture2D texture, string seed, int iterationsOfSmoothmap, bool useCopyOfMap)
        {
            WidthInBlocks = x;
            HeightInBlocks = y;
            _spriteBatch = sb;
            CellTexture = texture;
            mapGenerator = new RandomPlacement(WidthInBlocks, HeightInBlocks, randomFillPercent: 45); //change this when choosing algorithm for generation
            mapCleaner = new CellularAutomata(WidthInBlocks, HeightInBlocks, useCopyOfMap);
            Init(seed, iterationsOfSmoothmap);
        }

        private void Init(string seed, int iterationsOfSmoothmap)
        {
            Cells = new Cell[WidthInBlocks, HeightInBlocks];
            mapGenerator.Start(seed);
           
            for (int x = 0; x < WidthInBlocks; x++)
            {
                for (int y = 0; y < HeightInBlocks; y++)
                {
                    Vector2 pos = new Vector2(x * CellTexture.Width, y * CellTexture.Height);
                    Cells[x, y] = new Cell(pos, CellTexture, false);
                }
            }

            int[,] map = mapGenerator.GetMap();

            for(int i = 0; i < iterationsOfSmoothmap; i++)
            {
                map = mapCleaner.SmoothMap(map, iterationsOfSmoothmap);
            }

            int col = map.GetLength(0);
            int row = map.GetLength(1);
            for (int x = 0; x < col; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    if(map[x,y] == 1)
                    {
                        Cells[x, y].IsVisible = true;
                    }
                }
            }

            Cells[WidthInBlocks / 2, HeightInBlocks / 2].IsVisible = false;
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper move, int i)
        {
            if (move.IsDiagonalMove)
            {
                int stepsLeft = move.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = move.OneStep.X * Vector2.UnitX * stepsLeft;
                move.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(move.FurthestAvailableLocationSoFar, move.FurthestAvailableLocationSoFar + remainingHorizontalMovement, move.BoundingRectangle);

                Vector2 remainingVerticalMovement = move.OneStep.Y * Vector2.UnitY * stepsLeft;
                move.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(move.FurthestAvailableLocationSoFar, move.FurthestAvailableLocationSoFar + remainingVerticalMovement, move.BoundingRectangle);
            }

            return move.FurthestAvailableLocationSoFar;
        }

    }
}
