using System.Collections.Generic;
using System.Linq;

namespace Tank.Music
{
    class Playlist
    {
        private List<string> playlist;

        public Playlist(List<string> playlist)
        {
            playlist = RemoveDuplicates(playlist);
            this.playlist = playlist;
        }

        private List<string> RemoveDuplicates(List<string> playlist)
        {
            return playlist.Distinct().ToList();
        }

        public bool Contains(string song)
        {
            return playlist.Contains(song);
        }

        public void Add(string song)
        {
            if (Contains(song))
            {
                return;
            }
        }

        public void Remove(string song)
        {
            playlist.Remove(song);
        }

        public List<string> GetSongs()
        {
            return playlist;
        }
    }
}
