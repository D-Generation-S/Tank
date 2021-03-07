using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Settings;
using Tank.Music;

namespace Tank.Systems
{
    class MusicSystem : AbstractSystem
    {
        private readonly MusicManager musicManager;
        private readonly ApplicationSettings applicationSettings;
        private TimeSpan trackTime;
        private Song currentTrack;

        public MusicSystem(MusicManager musicManager, string playlist, ApplicationSettings applicationSettings)
        {
            musicManager.LoadPlaylist(playlist);
            this.musicManager = musicManager;
            this.applicationSettings = applicationSettings;
            MediaPlayer.Volume = applicationSettings.MusicVolume;
        }

        public override void Restore()
        {
            base.Restore();
            if (currentTrack != null)
            {
                MediaPlayer.Play(currentTrack, trackTime);
            }
            MediaPlayer.Volume = applicationSettings.MusicVolume;


        }

        public override void Suspend()
        {
            base.Suspend();
            trackTime = MediaPlayer.PlayPosition;
            MediaPlayer.Stop();
        }

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
