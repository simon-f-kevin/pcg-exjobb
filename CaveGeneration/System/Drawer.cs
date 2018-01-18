using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using CaveGeneration.Content_Generation.Goal_Placement;
using CaveGeneration.Content_Generation.Astar;
using System.Collections.Generic;
using CaveGeneration.Models.Characters;
using CaveGeneration.Content_Generation.Enemy_Placement;

namespace CaveGeneration.System
{
    class Drawer
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        Grid grid;
        Player player;
        Goal goal;
        List<Enemy> allEnemies;


        public Drawer(GraphicsDevice gd, SpriteBatch spriteBatch, Camera camera)
        {
            graphics = gd;
            this.spriteBatch = spriteBatch;
            this.camera = camera;

            allEnemies = new List<Enemy>();
        }

        public void GetDrawableObjects(Grid grid, Player player, Goal goal, List<Enemy> enemies)
        {
            this.grid = grid;
            this.player = player;
            this.goal = goal;
            allEnemies = enemies;
        }

        public void DrawEndOfGame(GameTime gameTime)
        {
            graphics.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            grid.Draw();
            player.Draw();
            goal.Draw();

            foreach (var enemy in allEnemies)
            {
                enemy.Draw();
            }

            if (!GameOverMessage.Equals(""))
            {
                spriteBatch.DrawString(font, GameOverMessage, new Vector2(player.Position.X, player.Position.Y - 50), Color.Black);
                spriteBatch.DrawString(font, "Press enter to return to menu", new Vector2(player.Position.X, player.Position.Y), Color.Black);
                spriteBatch.DrawString(font, "Press Esc to exit game", new Vector2(player.Position.X, player.Position.Y + 50), Color.Black);
            }

            spriteBatch.End();
        }

        public void DrawGamePlay(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            grid.Draw();
            player.Draw();
            goal.Draw();

            foreach (var enemy in allEnemies)
            {
                enemy.Draw();
            }

            spriteBatch.End();
        }

        public void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            spriteBatch.DrawString(font, "Press enter to start game", new Vector2(200, 200), Color.Black);
            spriteBatch.DrawString(font, "Press Esc to exit game", new Vector2(200, 200 + 50), Color.Black);


            spriteBatch.End();
        }
    }
}
