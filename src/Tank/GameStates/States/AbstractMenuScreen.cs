using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Abstract class for menu screens
    /// </summary>
    abstract class AbstractMenuScreen : BaseAbstractState
    {
        /// <summary>
        /// The data loader to use
        /// </summary>
        protected readonly IDataLoader<SpriteSheet> dataLoader;

        /// <summary>
        /// The gui sprite to use
        /// </summary>
        protected SpriteSheet guiSprite;

        /// <summary>
        /// The font to use for text
        /// </summary>
        protected SpriteFont baseFont;

        /// <summary>
        /// The amanger to load sprite sheets
        /// </summary>
        protected DataManager<SpriteSheet> spriteSetManager;

        /// <summary>
        /// The sound to make for button clicks
        /// </summary>
        protected SoundEffect buttonClick;

        /// <summary>
        /// The sound for buttons to make on mouse hover
        /// </summary>
        protected SoundEffect buttonHover;

        /// <summary>
        /// The main element to draw
        /// </summary>
        protected IGuiElement elementToDraw;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public AbstractMenuScreen()
            : this(new JsonTextureLoader())
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        public AbstractMenuScreen(IDataLoader<SpriteSheet> dataLoader)
        {
            this.dataLoader = dataLoader;
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            guiSprite = spriteSetManager.GetData("GuiSpriteSheet");
            baseFont = contentWrapper.Load<SpriteFont>("gameFont");
            buttonClick = contentWrapper.Load<SoundEffect>("Sound/Effects/UiClick");
            buttonHover = contentWrapper.Load<SoundEffect>("Sound/Effects/UiHover");
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (elementToDraw == null)
            {
                return;
            }
            elementToDraw.Update(gameTime);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            if (elementToDraw == null)
            {
                return;
            }
            TankGame.PublicGraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                GetScaleMatrix()
                );
            elementToDraw.Draw(gameTime);
            
            spriteBatch.End();
        }
    }
}
