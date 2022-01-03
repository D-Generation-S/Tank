using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataStructure.Settings;
using Tank.GameStates.States;
using TankEngine.Adapter;
using TankEngine.GameStates;
using TankEngine.Wrapper;

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
        public static string GameName = "TankGame";
        public static GraphicsDevice PublicGraphicsDevice;
        public static ContentManager PublicContentManager;
        public static IViewportAdapter PublicViewportAdapter;

        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
        }

        protected override void Initialize()
        {
            base.Initialize();
            ContentWrapper contentWrapper = new ContentWrapper(Content);
            //SaveableDataManager<ApplicationSettingsSingelton> settingManager = new SaveableDataManager<ApplicationSettingsSingelton>(contentWrapper, new JsonSettingLoader(), new JsonSettingSaver());
            //ApplicationSettingsSingelton settings = settingManager.GetData("settings");
            ApplicationSettingsSingelton.Instance.Load();
            InitResolution(ApplicationSettingsSingelton.Instance.Resolution.X, ApplicationSettingsSingelton.Instance.Resolution.Y, ApplicationSettingsSingelton.Instance.FullScreen);

            PublicGraphicsDevice = GraphicsDevice;
            PublicContentManager = Content;
            PublicViewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, ApplicationSettingsSingelton.Instance.FullScreen, 1920, 1080);
            PublicViewportAdapter.Reset();

            gameStateManager = new GameStateManager(contentWrapper, spriteBatch);
            gameStateManager.Add(new MainMenuState());
            IsMouseVisible = true;

            Window.ClientSizeChanged += (sender, eventData) =>
            {
                // Called to often, there should be a better solution!
                InitResolution(ApplicationSettingsSingelton.Instance.Resolution.X, ApplicationSettingsSingelton.Instance.Resolution.Y, ApplicationSettingsSingelton.Instance.FullScreen);
            };
        }

        /// <summary>
        /// Init the resolution
        /// </summary>
        /// <param name="windowWidth">The window width</param>
        /// <param name="windowHeight">The window height</param>
        /// <param name="fullscreen">Is the window full screen</param>
        private void InitResolution(int windowWidth, int windowHeight, bool fullscreen)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;

            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
        }

        /// <inheritdoc/>
        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <inheritdoc/>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!gameStateManager.StateAvailable)
            {
                Exit();
            }
            if (IsActive)
            {
                gameStateManager.Restore();
                gameStateManager.Update(gameTime);
                return;
            }

            gameStateManager.Suspend();
        }

        /// <inheritdoc/>
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