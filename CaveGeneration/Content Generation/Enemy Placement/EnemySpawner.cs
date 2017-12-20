using CaveGeneration.Models;
using CaveGeneration.Models.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Content_Generation.Enemy_Placement
{
    public class EnemySpawner
    {
        private List<Enemy> Enemies;
        private List<int> enemyIDs;
        private int nEnemies;
        private Texture2D enemyTexture;
        private Random rnd;
        private SpriteBatch spriteBatch;

        private Grid grid = Grid.Instance();

        public EnemySpawner(int numberOfEnemies, Texture2D enemyTexture, SpriteBatch spriteBatch)
        {
            Enemies = new List<Enemy>();
            enemyIDs = new List<int>();
            nEnemies = numberOfEnemies;
            this.enemyTexture = enemyTexture;
            this.spriteBatch = spriteBatch;
            rnd = new Random();
        }

        public void RunSpawner(Rectangle playerSpawnpoint)
        {
            for (int i = 0; i < nEnemies; i++)
            {
                var enemy = new Enemy(enemyTexture, spriteBatch, true)
                {
                    enemyID = i
                };
                enemy.SetSpawnPoint(GenerateSpawnPoint(enemy, playerSpawnpoint));
                enemyIDs.Add(i);
                Enemies.Add(enemy);
            }
        }

        
        public List<Enemy> GetEnemies()
        {
            return Enemies;
        }

        private Vector2 GenerateSpawnPoint(Enemy enemy, Rectangle playerSpawnpoint)
        {
            Vector2 spawnPoint;
            float X = rnd.Next(0, grid.Cells.GetLength(0));
            float Y = rnd.Next(0, grid.Cells.GetLength(1));
            Rectangle enemyRectangle = new Rectangle(new Point((int)X, (int)Y), new Point(enemyTexture.Width, enemyTexture.Height));
            
            while (grid.IsCollidingWithCell(enemyRectangle) && !enemyRectangle.Intersects(playerSpawnpoint)) //make sure enemy dont spawn inside walls or outside map
            {
                X = rnd.Next(0, grid.Cells.GetLength(0) * 20);
                Y = rnd.Next(0, grid.Cells.GetLength(1) * 20);
                enemyRectangle.X = (int)X;
                enemyRectangle.Y = (int)Y;
                if (!grid.IsCollidingWithCell(enemyRectangle)) break;
            }
            
            if(Enemies.Count >= 1)
            {
                foreach (Enemy prevEnemy in Enemies)
                {
                    if(prevEnemy.enemyID < enemy.enemyID)
                    {
                        Rectangle prevEnemyRectangle = new Rectangle(new Point((int)prevEnemy.Position.X, (int)prevEnemy.Position.Y), new Point(enemyTexture.Width, enemyTexture.Height));
                        if (enemyRectangle.Intersects(prevEnemyRectangle) || grid.IsCollidingWithCell(enemyRectangle) || enemyRectangle.Intersects(playerSpawnpoint))
                        {
                            X = X + enemyTexture.Width; //if two enemies spawn on eachother the new one moves to the right
                            enemyRectangle.X = (int)X;
                        }
                    }
                }
            }

            spawnPoint = new Vector2(X, Y);

            return spawnPoint;
        }

    }
}
