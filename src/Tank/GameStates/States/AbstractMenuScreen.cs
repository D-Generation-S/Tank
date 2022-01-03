using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Tank.DataManagement;
using Tank.DataManagement.Data;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Settings;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;
using Tank.Music;
using TankEngine.Wrapper;

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
        protected readonly IDataLoader<SpritesheetData> dataLoader;

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
        protected DataManager<SpritesheetData> spriteSetManager;

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
            : this(new JsonGameDataLoader<SpritesheetData>("Spritesheets"))
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="manager">The music manager to use</param>
        public AbstractMenuScreen(MusicManager manager)
            : this(new JsonGameDataLoader<SpritesheetData>("Spritesheets"), manager)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        public AbstractMenuScreen(IDataLoader<SpritesheetData> dataLoader)
            : this(dataLoader, null)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="musicManager">The music manager to use</param>
        public AbstractMenuScreen(IDataLoader<SpritesheetData> dataLoader, MusicManager musicManager)
        {
            this.dataLoader = dataLoader;
            this.musicManager = musicManager;
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            spriteSetManager = new DataManager<SpritesheetData>(dataLoader);
            if (musicManager == null)
            {
                musicManager = new MusicManager(contentWrapper, new DataManager<Music.Playlist>(new JsonPlaylistLoader(), true));
            }
            MediaPlayer.Volume = ApplicationSettingsSingelton.Instance.MusicVolume;
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            guiSprite = spriteSetManager.GetData("GuiSpriteSheet", data =>
            {
                Texture2D texture = contentWrapper.Load<Texture2D>(data.TextureName);
                return new SpriteSheet(texture, data.SingleImageSize.GetPoint(), data.DistanceBetweenImages, data.Patterns);
            });
            baseFont = contentWrapper.Load<SpriteFont>("gameFont");
            buttonClick = contentWrapper.Load<SoundEffect>("Sound/Effects/UiClick");
            buttonHover = contentWrapper.Load<SoundEffect>("Sound/Effects/UiHover");
            if (!musicManager.PlaylistLoaded)
            {
                musicManager.LoadPlaylist("MenuMusic");
            }
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            base.Restore();
            MediaPlayer.Volume = ApplicationSettingsSingelton.Instance.MusicVolume;
            UpdateUiEffects(ApplicationSettingsSingelton.Instance.EffectVolume);
            if (ApplicationSettingsSingelton.Instance.LastPlayedSong == null || MediaPlayer.State == MediaState.Playing)
            {
                return;
            }
            MediaPlayer.Play(ApplicationSettingsSingelton.Instance.LastPlayedSong, ApplicationSettingsSingelton.Instance.LastTimeSpan);
        }

        /// <inheritdoc/>
        public override void Suspend()
        {
            base.Suspend();
            ApplicationSettingsSingelton.Instance.LastTimeSpan = MediaPlayer.PlayPosition;
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Update the ui effectss
        /// </summary>
        /// <param name="newVolume">The new volume to use</param>
        protected void UpdateUiEffects(float newVolume)
        {
            UpdateUiEffects(elementToDraw, newVolume);
        }

        /// <summary>
        /// Update the ui effectss
        /// </summary>
        /// <param name="guiElement">The gui element to update the effect volume</param>
        /// <param name="newVolume">The new volume to use</param>
        protected void UpdateUiEffects(IGuiElement guiElement, float newVolume)
        {
            if (guiElement is Panel panel)
            {
                panel.Container.ForEach(item =>
                {
                    if (item is VisibleUiElement visibleUiElement)
                    {
                        visibleUiElement.SetEffectVolume(newVolume);
                    }
                });
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                ApplicationSettingsSingelton.Instance.LastPlayedSong = musicManager.GetNextSong();
                MediaPlayer.Play(ApplicationSettingsSingelton.Instance.LastPlayedSong);
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
