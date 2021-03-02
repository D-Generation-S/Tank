using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tank.GameStates;
using Tank.GameStates.Data;
using Tank.GameStates.States;
using Tank.Map.Generators;
using Tank.Randomizer;
using Tank.Wrapper;

namespace Tank
{
    /// <summary>
    /// Main entry point of the game
    /// </summary>
    public class TankGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateManager gameStateManager;
        public static GraphicsDevice PublicGraphicsDevice;
        public static ContentManager PublicContentManager;

        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            gameStateManager = new GameStateManager(new ContentWrapper(Content), spriteBatch);
            GameSettings settings = new GameSettings(0.098f, 0.3f, 4, "MoistContinentalSpritesheet"); 
            gameStateManager.Add(new GameLoadingScreen(new MidpointDisplacementGenerator(GraphicsDevice, 900 / 4, 0.5f, new SystemRandomizer()), settings));
            PublicGraphicsDevice = GraphicsDevice;
            PublicContentManager = Content;
            InitResolution(1440, 900);

            IsMouseVisible = true;
        }

        private void InitResolution(int windowWidth, int windowHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                gameStateManager.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (IsActive)
            {
                gameStateManager.Draw(gameTime);
            }

        }
    }
}