using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using Tank.DataManagement;
using TankEngine.Randomizer;
using TankEngine.Wrapper;

namespace TankEngine.Music
{
    /// <summary>
    /// The music manager class to get the songs from the playlist
    /// </summary>
    public class MusicManager
    {
        /// <summary>
        /// Is there a playlist loaded right now
        /// </summary>
        public bool PlaylistLoaded => songs.Count > 0;

        /// <summary>
        /// Get the name of the playlist
        /// </summary>
        public string PlaylistName { get; private set; }

        /// <summary>
        /// The content wrapper to use
        /// </summary>
        private readonly ContentWrapper contentWrapper;

        /// <summary>
        /// The playlist loader to use
        /// </summary>
        private readonly DataManager<Playlist> playlistLoader;

        /// <summary>
        /// A list with all the songs
        /// </summary>
        private readonly List<Song> songs;

        /// <summary>
        /// The current mixed playlist to use
        /// </summary>
        protected Stack<Song> currentMixedPlaylist;

        /// <summary>
        /// The randommizer to use
        /// </summary>
        private readonly IRandomizer randomizer;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="playlistLoader">The playlist loader to use</param>
        public MusicManager(ContentWrapper contentWrapper, DataManager<Playlist> playlistLoader)
        {
            this.contentWrapper = contentWrapper;
            this.playlistLoader = playlistLoader;

            randomizer = new SystemRandomizer();
            currentMixedPlaylist = new Stack<Song>();
            songs = new List<Song>();
        }

        /// <summary>
        /// Load a new playlist
        /// </summary>
        /// <param name="name">The name of the playlist</param>
        public void LoadPlaylist(string name)
        {
            if (playlistLoader == null)
            {
                return;
            }

            Playlist playlist = playlistLoader.GetData(name);
            if (playlist == null)
            {
                return;
            }

            foreach (string songname in playlist.GetSongs())
            {
                try
                {
                    Song loadedSongs = contentWrapper.Load<Song>(songname);
                    songs.Add(loadedSongs);
                }
                catch (Exception)
                {

                }
            }
            PlaylistName = name;
        }

        /// <summary>
        /// Create the song list
        /// </summary>
        protected void CreateSongList()
        {
            int count = songs.Count;
            int last = count - 1;
            List<Song> copyList = songs;
            for (int i = 0; i < last; i++)
            {
                int random = randomizer.GetNewIntNumber(i, count);
                Song tmpSong = copyList[i];
                copyList[i] = copyList[random];
                copyList[random] = tmpSong;
            }

            foreach (Song song in copyList)
            {
                currentMixedPlaylist.Push(song);
            }
        }

        /// <summary>
        /// Get the next song for playing
        /// </summary>
        /// <returns>The next song or null</returns>
        public Song GetNextSong()
        {
            if (currentMixedPlaylist.Count == 0)
            {
                CreateSongList();
            }
            return currentMixedPlaylist.Count > 0 ? currentMixedPlaylist.Pop() : null;
        }

    }
}
