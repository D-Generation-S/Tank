using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using Tank.DataStructure.Settings;
using TankEngine.EntityComponentSystem.Systems;
using TankEngine.Music;

namespace Tank.Systems
{
    /// <summary>
    /// A system to play music in the game
    /// </summary>
    class MusicSystem : AbstractSystem
    {
        /// <summary>
        /// The music manager to use
        /// </summary>
        private readonly MusicManager musicManager;


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
        public MusicSystem(MusicManager musicManager, string playlist)
        {
            musicManager.LoadPlaylist(playlist);
            this.musicManager = musicManager;
            MediaPlayer.Volume = ApplicationSettingsSingelton.Instance.MusicVolume;
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            base.Restore();
            if (currentTrack != null)
            {
                MediaPlayer.Play(currentTrack, trackTime);
            }
            MediaPlayer.Volume = ApplicationSettingsSingelton.Instance.MusicVolume;


        }

        /// <inheritdoc/>
        public override void Suspend()
        {
            base.Suspend();
            trackTime = MediaPlayer.PlayPosition;
            MediaPlayer.Stop();
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
