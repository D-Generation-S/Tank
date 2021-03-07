using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;
using Tank.Music;
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
        /// The manager to use
        /// </summary>
        protected MusicManager musicManager;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public AbstractMenuScreen()
            : this(new JsonTextureLoader())
        {

        }

        public AbstractMenuScreen(MusicManager manager)
            : this(new JsonTextureLoader(), manager)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        public AbstractMenuScreen(IDataLoader<SpriteSheet> dataLoader)
            :this(dataLoader, null)
        {
            
        }

        public AbstractMenuScreen(IDataLoader<SpriteSheet> dataLoader, MusicManager manager)
        {
            this.dataLoader = dataLoader;
            this.musicManager = manager;
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);
            if (musicManager == null)
            {
                musicManager = new MusicManager(contentWrapper, new DataManager<Music.Playlist>(contentWrapper, new JsonPlaylistLoader(), true));
            }
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            guiSprite = spriteSetManager.GetData("GuiSpriteSheet");
            baseFont = contentWrapper.Load<SpriteFont>("gameFont");
            buttonClick = contentWrapper.Load<SoundEffect>("Sound/Effects/UiClick");
            buttonHover = contentWrapper.Load<SoundEffect>("Sound/Effects/UiHover");
            if (!musicManager.PlaylistLoaded)
            {
                musicManager.LoadPlaylist("MenuMusic");
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            //MediaPlayer.Volume = 1.0f;
            if (MediaPlayer.State == MediaState.Stopped)
            {
                Song song = musicManager.GetNextSong();
                MediaPlayer.Play(song);
            }
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
