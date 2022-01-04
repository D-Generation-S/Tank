using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TankEngine.Music
{
    /// <summary>
    /// This class represents a playlist
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// The stored playlist
        /// </summary>
        [JsonPropertyName("Playlist")]
        public List<string> PlayList { get; set; }

        public Playlist()
        {
            PlayList = new List<string>();
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="playlist">The playlist to use</param>
        public Playlist(List<string> playlist)
        {
            playlist = RemoveDuplicates(playlist);
            PlayList = playlist;
        }

        /// <summary>
        /// Remove duplicate songs
        /// </summary>
        /// <param name="playlist">The playlist to remove duplicates from</param>
        /// <returns>A list with all the songs to load</returns>
        private List<string> RemoveDuplicates(List<string> playlist)
        {
            return playlist.Distinct().ToList();
        }

        /// <summary>
        /// Check if a song exists in the playlist
        /// </summary>
        /// <param name="songName">The name of the song to search</param>
        /// <returns>True if song is in playlist</returns>
        public bool Contains(string songName)
        {
            return PlayList.Contains(songName);
        }

        /// <summary>
        /// Add a new song to the playlist
        /// </summary>
        /// <param name="songName">The name of the song to add</param>
        public void Add(string songName)
        {
            if (Contains(songName))
            {
                return;
            }
        }

        /// <summary>
        /// Remove a song from the playlist
        /// </summary>
        /// <param name="songName">The name of the song to remove</param>
        public void Remove(string songName)
        {
            PlayList.Remove(songName);
        }

        /// <summary>
        /// Get all the songs in the list
        /// </summary>
        /// <returns>A list with all the songs</returns>
        public List<string> GetSongs()
        {
            return PlayList;
        }
    }
}
