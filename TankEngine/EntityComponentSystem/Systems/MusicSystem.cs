using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.Music;

namespace TankEngine.EntityComponentSystem.Systems
{
    /// <summary>
    /// A system to play music in the game
    /// </summary>
    public class MusicSystem : AbstractSystem
    {
        /// <summary>
        /// The music manager to use
        /// </summary>
        private readonly MusicManager musicManager;

        /// <summary>
        /// The music volume for the system
        /// </summary>
        private float musicVolume;

        /// <summary>
        /// The current track time
        /// </summary>
        private TimeSpan trackTime;

        /// <summary>
        /// The current track to play
        /// </summary>
        private Song currentTrack;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="musicManager">The music manager to use</param>
        /// <param name="playlist">The playlist to load</param>
        public MusicSystem(MusicManager musicManager, string playlist, float musicVolume)
        {
            musicManager.LoadPlaylist(playlist);
            this.musicManager = musicManager;
            this.musicVolume = MathHelper.Clamp(musicVolume, 0, 1);
            MediaPlayer.Volume = musicVolume;
        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent<VolumeChangedEvent>(this);
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            base.Restore();
            if (currentTrack != null)
            {
                MediaPlayer.Play(currentTrack, trackTime);
            }
            MediaPlayer.Volume = musicVolume;
        }

        /// <inheritdoc/>
        public override void Suspend()
        {
            base.Suspend();
            trackTime = MediaPlayer.PlayPosition;
            MediaPlayer.Stop();
        }

        /// <inheritdoc/>
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            if (eventArgs is VolumeChangedEvent volumeEvent)
            {
                switch (volumeEvent.VolumeType)
                {
                    case Enums.VolumeTypeEnum.Music:
                        musicVolume = volumeEvent.NewVolume;
                        break;
                    default:
                        break;
                }
                MediaPlayer.Volume = musicVolume;
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MediaPlayer.State == MediaState.Stopped && musicManager.PlaylistLoaded)
            {
                currentTrack = musicManager.GetNextSong();
                MediaPlayer.Play(currentTrack);
            }
        }
    }
}
