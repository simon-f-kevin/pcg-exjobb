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
    class Updater
    {

        public void UpdateEndOfGame(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                gameState = GameState.MainMenu;

            }
        }

        public void UpdateGameplay(GameTime gameTime)
        {
            playerPosition = player.Position;
            playerRectangle.X = (int)playerPosition.X;
            playerRectangle.Y = (int)playerPosition.Y;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (goal.BoundingRectangle.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
            {
                GameOverMessage = "You Win!";

                gameState = GameState.GameOver;
            }

            // TODO: Add your update logic here
            player.Update(gameTime);
            camera.Update(gameTime, this);

            foreach (var enemy in allEnemies)
            {
                enemy.Update(gameTime);
                if (playerRectangle.Intersects(new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Texture.Width, enemy.Texture.Height)))
                {

                    if (!player.hurt)
                    {
                        player.dealDamage();
                    }
                    if (!player.isAlive())
                    {
                        GameOverMessage = "You Lose!";

                        gameState = GameState.GameOver;
                    }

                }
            }
        }

        public void UpdateMainMenu(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                CreateMap(mapWidth, mapHeight, useCopyOfMap: useCopy);
                gameState = GameState.Playing;

            }
        }
    }
}
