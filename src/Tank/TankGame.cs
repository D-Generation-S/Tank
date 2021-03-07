using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using Tank.Adapter;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.GameStates;
using Tank.GameStates.Data;
using Tank.GameStates.States;
using Tank.Map.Generators;
using Tank.Music;
using Tank.Randomizer;
using Tank.Wrapper;

namespace Tank
{
    /// <summary>
    /// Main entry point of the game
    /// </summary>
    public class TankGame : Game
    {

        private IViewportAdapter viewportAdapter;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateManager gameStateManager;
        public static GraphicsDevice PublicGraphicsDevice;
        public static ContentManager PublicContentManager;
        public static IViewportAdapter PublicViewportAdapter;
        

        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitResolution(1280, 720);
            PublicGraphicsDevice = GraphicsDevice;
            PublicContentManager = Content;
            PublicViewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080); ;

            gameStateManager = new GameStateManager(new ContentWrapper(Content), spriteBatch);
            gameStateManager.Add(new MainMenuState());
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

            ContentWrapper contentWrapper = new ContentWrapper(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsActive)
            {
                gameStateManager.Update(gameTime);
                if (!gameStateManager.StateAvailable)
                {
                    Exit();
                }
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