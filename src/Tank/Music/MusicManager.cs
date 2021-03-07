using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using Tank.DataManagement;
using Tank.Interfaces.Randomizer;
using Tank.Randomizer;
using Tank.Wrapper;

namespace Tank.Music
{
    class MusicManager
    {
        public bool PlaylistLoaded => songs.Count > 0;
        public string PlaylistName { get; private set; }

        private readonly ContentWrapper contentWrapper;
        private readonly DataManager<Playlist> playlistLoader;
        
        private readonly List<Song> songs;

        protected Stack<Song> currentMixedPlaylist;
        private readonly IRandomizer randomizer;


        public MusicManager(ContentWrapper contentWrapper, DataManager<Playlist> playlistLoader)
        {
            this.contentWrapper = contentWrapper;
            this.playlistLoader = playlistLoader;

            randomizer = new SystemRandomizer();
            currentMixedPlaylist = new Stack<Song>();
            songs = new List<Song>();
        }

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

            foreach(string songname in playlist.GetSongs())
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
