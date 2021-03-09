using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Settings;
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

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="manager">The music manager to use</param>
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

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="musicManager">The music manager to use</param>
        public AbstractMenuScreen(IDataLoader<SpriteSheet> dataLoader, MusicManager musicManager)
        {
            this.dataLoader = dataLoader;
            this.musicManager = musicManager;
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            base.Initialize(contentWrapper, spriteBatch, applicationSettings);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);
            if (musicManager == null)
            {
                musicManager = new MusicManager(contentWrapper, new DataManager<Music.Playlist>(contentWrapper, new JsonPlaylistLoader(), true));
            }
            MediaPlayer.Volume = settings.MusicVolume;
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
        public override void Restore()
        {
            base.Restore();
            MediaPlayer.Volume = settings.MusicVolume;
            UpdateUiEffects(settings.EffectVolume);
            if (settings.LastPlayedSong == null || MediaPlayer.State == MediaState.Playing)
            {
                return;
            }
            MediaPlayer.Play(settings.LastPlayedSong, settings.LastTimeSpan);
        }

        /// <inheritdoc/>
        public override void Suspend()
        {
            base.Suspend();
            settings.LastTimeSpan = MediaPlayer.PlayPosition;
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
                settings.LastPlayedSong = musicManager.GetNextSong();
                MediaPlayer.Play(settings.LastPlayedSong);
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
