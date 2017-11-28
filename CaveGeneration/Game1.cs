using CaveGeneration.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using CaveGeneration.Content_Generation.Goal_Placement;

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

        Grid grid;
        Character player;
        Rectangle spawnPoint;
        Goal goal;
        StartAndGoalPlacer startAndGoalPlacer;

        public Vector2 playerPosition;
        public Rectangle playerRectangle;

        string seed;
        int blockHeight;
        int blockWidth;

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

            // Sets the window-size
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - 100;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            //Sets the block size
            blockHeight = 20;
            blockWidth = 20;

            camera = new Camera(GraphicsDevice.Viewport);

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
            block = CreateTexture(graphics.GraphicsDevice, blockWidth, blockHeight, pixel => Color.Gray);
            characterTexture = Content.Load<Texture2D>("skeleton_2");
            goalTexture = CreateTexture(graphics.GraphicsDevice, blockWidth, blockHeight, pixel => Color.Gold);
            spawnPoint = new Rectangle(new Point(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2), new Point(characterTexture.Width, characterTexture.Height));
            
            grid = Grid.CreateNewGrid(200, 20, spriteBatch, block, seed, 1);
            goal = new Goal(new Vector2(0, 0), goalTexture, spriteBatch);
            startAndGoalPlacer = new StartAndGoalPlacer(goal, characterTexture, graphics);
            spawnPoint = startAndGoalPlacer.GetSpawnPosition();
            player = new Character(characterTexture, new Vector2(spawnPoint.X, spawnPoint.Y), spriteBatch);
            goal = startAndGoalPlacer.GenerateReachableGoalPosition();

            playerRectangle = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Texture.Width, player.Texture.Height);
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
            playerPosition = player.Position;
            playerRectangle.X = (int)playerPosition.X;
            playerRectangle.Y = (int)playerPosition.Y;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!player.Alive)
            {
                Exit();
            }
            // TODO: Add your update logic here
            player.Update(gameTime);
            camera.Update(gameTime, this);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            //spriteBatch.Draw(block, new Vector2(100,1));
            grid.Draw();
            player.Draw();
            goal.Draw();
            spriteBatch.End();

            
            base.Draw(gameTime);
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

        

    }
}
