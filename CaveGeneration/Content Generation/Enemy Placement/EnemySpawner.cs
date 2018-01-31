using CaveGeneration.Models;
using CaveGeneration.Models.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using CaveGeneration.Content_Generation.Parameter_Settings;
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
        private Texture2D staticEnemyTexture;
        private Random rnd;
        private SpriteBatch spriteBatch;
        private Rectangle forbiddenZone;

        private Settings settings;
        private int distanceToMove;

        private Grid grid = Grid.Instance();

        public EnemySpawner(Settings settings, Texture2D enemyTexture, Texture2D staticEnemyTexture, SpriteBatch spriteBatch)
        {
            this.settings = settings;
            Enemies = new List<Enemy>();
            enemyIDs = new List<int>();
            nEnemies = settings.EnemyCount;
            this.enemyTexture = enemyTexture;
            this.staticEnemyTexture = staticEnemyTexture;
            this.spriteBatch = spriteBatch;
            distanceToMove = settings.DistanceBetweenEnemies;
            rnd = new Random(settings.Seed.GetHashCode());
        }

        public void RunEnemySpawner(Rectangle playerSpawnpoint)
        {
            for (int i = 0; i < nEnemies; i++)
            {
                Enemy enemy;
                if (rnd.Next(1, 101) <= settings.StaticEnemyChance)
                {
                    enemy = new StaticEnemy(staticEnemyTexture, spriteBatch)
                    {
                        enemyID = i
                    };
                }
                else
                {
                    enemy = new Enemy(enemyTexture, spriteBatch, settings.EnemiesCanJump)
                    {
                        enemyID = i
                    };
                }
                enemy.SetSpawnPoint(GenerateEnemySpawnpoint(enemy, playerSpawnpoint));
                enemyIDs.Add(i);
                Enemies.Add(enemy);
            }
        }
        
        public List<Enemy> GetEnemies()
        {
            return Enemies;
        }

        //TODO: move enemy spawn further away form player
        private Vector2 GenerateEnemySpawnpoint(Enemy enemy, Rectangle playerSpawnpoint)
        {
            Vector2 enemySpawnpoint;
            float X = rnd.Next(0, grid.Cells.GetLength(0));
            float Y = rnd.Next(0, grid.Cells.GetLength(1));
            forbiddenZone = new Rectangle(new Point(playerSpawnpoint.Left, playerSpawnpoint.Bottom), new Point(enemyTexture.Width * 3, enemyTexture.Height * 3));
            Rectangle enemyRectangle = new Rectangle(new Point((int)X, (int)Y), new Point(enemyTexture.Width, enemyTexture.Height));
            
            while (grid.IsCollidingWithCell(enemyRectangle) && !enemyRectangle.Intersects(forbiddenZone)) //make sure enemy dont spawn inside walls or outside map
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
                        while (enemyRectangle.Intersects(prevEnemyRectangle) || grid.IsCollidingWithCell(enemyRectangle) || enemyRectangle.Intersects(forbiddenZone))
                        {
                            X = X + enemy.Texture.Width + distanceToMove; //if two enemies spawn on eachother the new one moves to the right
                            enemyRectangle.X = (int)X;
                        }
                    }
                }
            }

            enemySpawnpoint = new Vector2(X, Y);

            return enemySpawnpoint;
        }

    }
}
