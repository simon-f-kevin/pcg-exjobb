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
using CaveGeneration.Content_Generation.Parameter_Settings;
using Microsoft.Xna.Framework.Media;
using CaveGeneration.Content_Generation.Pitfall_Placement;

namespace CaveGeneration
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;

        Texture2D block;
        Texture2D characterTexture;
        Texture2D goalTexture;
        Texture2D enemyTexture;
        Texture2D staticEnemyTexture;

        Song backgroundMusic;
        SpriteFont font;

        Grid grid;
        Player player;
        Rectangle spawnPoint;
        Goal goal;
        StartAndGoalPlacer startAndGoalPlacer;
        EnemySpawner enemySpawner;
        HealthCounter hpCounter;
        PitfallSpawner pitfallSpawner;

        List<Enemy> allEnemies;

        public Vector2 playerPosition;
        public Rectangle playerRectangle;

        string seed;
        string originalSeed;
        int blockHeight;
        int blockWidth;

        string GameOverMessage;
        int numberOfGames;
        int remainingLives;
        int totalLives;
        int gamesWon;

        bool musicIsPlaying;
        GameState gameState;

        KeyboardState previousState;

        //Create map parameters
        int mapWidth = 64;
        int mapHeight = 16;
        bool useCopy = true;


        Rectangle StageArea;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Set your seed. Leave empty if you want a random map
            seed = "";
            originalSeed = seed;

            // Sets the window-size
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - 100;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            //Sets the block size
            blockHeight = 20;
            blockWidth = 20;

            camera = new Camera(GraphicsDevice.Viewport);

            GameOverMessage = "";
            numberOfGames = 0;
            totalLives = 0;
            remainingLives = 0;
            gamesWon = 0;
            
            gameState = GameState.MainMenu;
            musicIsPlaying = false;
            MediaPlayer.IsRepeating = true;

            StageArea = new Rectangle(0, 0, mapWidth * blockWidth, mapHeight * blockHeight);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            backgroundMusic = Content.Load<Song>("cave-music");
            hpCounter = new HealthCounter(spriteBatch, font);
            block = CreateTexture(graphics.GraphicsDevice, blockWidth, blockHeight, pixel => Color.White);
            characterTexture = Content.Load<Texture2D>("sprite-girl");
            enemyTexture = Content.Load<Texture2D>("enemy");
            staticEnemyTexture = Content.Load<Texture2D>("static-enemy");
            goalTexture = CreateTexture(graphics.GraphicsDevice, blockWidth, blockHeight, pixel => Color.Gold);
            spawnPoint = new Rectangle(new Point(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), new Point(characterTexture.Width, characterTexture.Height));
            goal = new Goal(new Vector2(0, 0), goalTexture, spriteBatch);
            CreateMap(mapWidth, mapHeight, useCopyOfMap: useCopy);
            playerRectangle = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height);

            if (!musicIsPlaying)
            {
                MediaPlayer.Play(backgroundMusic);
                musicIsPlaying = true;
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateUniversal(gameTime);
            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.GameOver:
                    UpdateEndOfGame(gameTime);
                    break;
                case GameState.StatScreen:
                    UpdateStatScreen(gameTime);
                    break;
                case GameState.Tutorial:
                    UpdateTutorial(gameTime);
                    break;
            }
            base.Update(gameTime);
            
        }


        private void UpdateUniversal(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.M) && !previousState.IsKeyDown(Keys.M))
            {
                if (musicIsPlaying == false)
                {
                    MediaPlayer.Resume();
                    musicIsPlaying = true;
                }
                else
                {
                    MediaPlayer.Pause();
                    musicIsPlaying = false;
                }

            }
            previousState = Keyboard.GetState();
        }

        /// <summary>
        /// This is the update loop for the main menu
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateMainMenu(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //CreateMap(mapWidth, mapHeight, useCopyOfMap: useCopy);
                gameState = GameState.Playing;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                gameState = GameState.Tutorial;
            }
        }

        /// <summary>
        /// Update loop for playing the game.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateGameplay(GameTime gameTime)
        {
            playerPosition = player.Position;
            playerRectangle.X = (int)playerPosition.X;
            playerRectangle.Y = (int)playerPosition.Y;

               

            if (goal.BoundingRectangle.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height)))
            {
                GameOverMessage = "You Win!";
                
                gameState = GameState.GameOver;
            }

            // TODO: Add your update logic here
            hpCounter.Update(player.GetHp());
            player.Update(gameTime);
            camera.Update(gameTime, this);

            if (!playerRectangle.Intersects(StageArea))
            {
                while (player.IsAlive())
                    player.DealDamage();
                GameOverMessage = "You Lose!";
                gameState = GameState.GameOver;
            }

            foreach (var enemy in allEnemies)
            {
                enemy.Update(gameTime);
                if (playerRectangle.Intersects(new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Texture.Width, enemy.Texture.Height)))
                {
                    
                    if (!player.hurt)
                    {
                        player.DealDamage();
                    }
                  
                }
            }
            if (!player.IsAlive())
            {
                GameOverMessage = "You Lose!";

                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// The update loop for the end of game screen
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateEndOfGame(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (numberOfGames < 4)
                {
                    GetStats();
                    RestartGame();
                }
                else if (numberOfGames == 4)
                {
                    GetStats();
                    gameState = GameState.StatScreen;
                }
                else gameState = GameState.GameOver;
            }
        }

        private void UpdateStatScreen(GameTime gameTime)
        {
            musicIsPlaying = false;
            MediaPlayer.Stop();
        }

        private void UpdateTutorial(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                gameState = GameState.MainMenu;
            }
        }

        /// <summary>
        /// This method gets the stats from your current game and adds them to total stats
        /// </summary>
        private void GetStats()
        {
            remainingLives = player.GetHp();
            totalLives += remainingLives;
            if (GameOverMessage.Equals("You Win!"))
            {
                gamesWon++;
            }
        }

        /// <summary>
        /// This method crates a new map to let the player play the game again. 
        /// </summary>
        private void RestartGame()
        {
            numberOfGames++;
            CreateMap(mapWidth, mapHeight, useCopy);
            gameState = GameState.Playing;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Playing:
                    DrawGamePlay(gameTime);
                    break;
                case GameState.GameOver:
                    DrawEndOfGame(gameTime);
                    break;
                case GameState.StatScreen:
                    DrawStatScreen(gameTime);
                    break;
                case GameState.Tutorial:
                    DrawTutorial(gameTime);
                    break;
            }

            base.Draw(gameTime);
            
        }


        private void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            spriteBatch.DrawString(font, "Press Enter to start game", new Vector2(200, 200), Color.Navy);
            spriteBatch.DrawString(font, "Press T for tutorial", new Vector2(200, 250), Color.Navy);
            spriteBatch.DrawString(font, "Press Esc to exit game", new Vector2(200, 300), Color.Navy);
            spriteBatch.DrawString(font, "Nick & Simon 2018", new Vector2(200, 400), Color.Navy);


            spriteBatch.End();
        }

        private void DrawGamePlay(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            grid.Draw();
            player.Draw();
            goal.Draw();
            hpCounter.Draw(player.Position);

            foreach (var enemy in allEnemies)
            {
                enemy.Draw();
            }

            spriteBatch.End();
        }

        private void DrawEndOfGame(GameTime gameTime)
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

            if (!GameOverMessage.Equals(""))
            {
                spriteBatch.DrawString(font, GameOverMessage, new Vector2(player.Position.X, player.Position.Y - 50), Color.Navy);
                spriteBatch.DrawString(font, "Game " + (numberOfGames + 1) + " of 5 ", new Vector2(player.Position.X + 250, player.Position.Y - 50), Color.Navy);
                spriteBatch.DrawString(font, "Press enter to continue", new Vector2(player.Position.X, player.Position.Y), Color.Navy);
                spriteBatch.DrawString(font, "Press Esc to exit game", new Vector2(player.Position.X, player.Position.Y + 50), Color.Navy);
            }

            spriteBatch.End();
        }

        private void DrawStatScreen(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            spriteBatch.DrawString(font, "You won " + gamesWon + " games out of 5!", new Vector2(player.Position.X, player.Position.Y - 50), Color.Navy);
            spriteBatch.DrawString(font, "You got " + totalLives + " points!", new Vector2(player.Position.X, player.Position.Y), Color.Navy);
            spriteBatch.DrawString(font, "Press Esc to exit game", new Vector2(player.Position.X, player.Position.Y + 50), Color.Navy);
            spriteBatch.DrawString(font, "Music by KyBOz / www.oreitia.com", new Vector2(player.Position.X, player.Position.Y + 100), Color.Navy);

            spriteBatch.End();
        }

        private void DrawTutorial(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            spriteBatch.DrawString(font, "Use arrow keys to move or left thumbstick on controllers", new Vector2(100, 100), Color.Navy);
            spriteBatch.DrawString(font, "Press Up arrow, Space or A on controllers to jump", new Vector2(100, 150), Color.Navy);
            spriteBatch.DrawString(font, "Hold X or RT on controllers and jump to perform", new Vector2(100, 200), Color.Navy);
            spriteBatch.DrawString(font, "a super jump(This drains 1 health)", new Vector2(100, 250), Color.Navy);
            spriteBatch.DrawString(font, "Press M to mute the music", new Vector2(100, 300), Color.Navy);
            spriteBatch.DrawString(font, "Press Q to skip current level", new Vector2(100, 350), Color.Navy);
            spriteBatch.DrawString(font, "Press Esc to exit the game at any time", new Vector2(100, 400), Color.Navy);
            
            spriteBatch.DrawString(font, "Press Enter to return to main menu", new Vector2(100, 450), Color.Navy);


            spriteBatch.End();
        }

        private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, System.Func<int, Color> paint)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }

        private void CreateMap(int mapWidthInBlocks, int mapHeightInBlocks, bool useCopyOfMap)
        {

            Settings settings = PredefinedSettings.settings1;


            bool solveable = true;
            do
            {
                Grid.ClearInstance();
                grid = Grid.CreateNewGrid(mapWidthInBlocks, mapHeightInBlocks, spriteBatch, block, seed, settings);

                if (settings.IncrementDifficulty && remainingLives >= 2)
                {
                    DifficultyIncrementer.Increment(settings, seed);
                }

                pitfallSpawner = new PitfallSpawner(settings);
                pitfallSpawner.GeneratePitfalls();

                startAndGoalPlacer = new StartAndGoalPlacer(goal, characterTexture, graphics, settings);
                enemySpawner = new EnemySpawner(settings, enemyTexture, staticEnemyTexture, spriteBatch);
                spawnPoint = startAndGoalPlacer.GetSpawnPosition();
                enemySpawner.RunEnemySpawner(spawnPoint);
                player = new Player(characterTexture, new Vector2(spawnPoint.X, spawnPoint.Y), spriteBatch, settings);
                startAndGoalPlacer.SetPlayer(player);
                allEnemies = enemySpawner.GetEnemies();

                try
                {
                    goal = startAndGoalPlacer.GenerateReachableGoalPosition();
                    solveable = true;
                }
                catch (NotSolveableException ex)
                {
                    //if the map is not solveable we generate a new random seed and try again
                    if (ex.Message.Equals("Not solveable"))
                    {
                        solveable = false;
                        Console.WriteLine(seed);
                        Console.WriteLine(originalSeed);
                        int tmp;
                        if (!originalSeed.Equals(""))
                        {
                            tmp = new Random(seed.GetHashCode()).Next();
                        }
                        else
                        {
                            tmp = new Random().Next();
                        }
                        seed = tmp.GetHashCode().ToString();
                    }
                }
            }
            while (!solveable);
            
        }

    }
}
