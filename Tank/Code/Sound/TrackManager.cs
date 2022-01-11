using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Code.General;
using System.Text;
using Microsoft.Xna.Framework.Media;
//using System.Threading;
using System.Timers;
using Tank.Enums;

namespace Tank.Code.Sound
{

    public class TrackManager
    {
        private TrackType _category;
        private TrackMode _mode;
        public TrackType Category
        {
            get
            {
                return _category;
            }
            set
            {
                TrackType previous = _category;
                _category = value;
                if (previous != _category)
                {
                    CreateCategoryIndexes();
                }
            }
        }
        public TrackMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                SortCategoryIndexes();
            }
        }

        private static TrackManager _instance;
        public static TrackManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TrackManager();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public List<Track> TrackCollection
        {
            get;
            set;
        }
        public List<int> CategoryTrackIndexes
        {
            get; set;
        }
        private int _currentTrackIndexIndex = 0;

        private float _maxVolume;
        public float MaxVolume
        {
            get
            {
                return _maxVolume;
            }
            set
            {
                _maxVolume = value;
                if (_currentVolume > _maxVolume)
                    _currentVolume = _maxVolume;
                MediaPlayer.Volume = _currentVolume;
            }
        }
        private float _currentVolume;
        private float _stepSize;

        public int CurrentVolume
        {
            get
            {
                return (int)(_currentVolume * 100);
            }
            set
            {
                _maxVolume = (float)value / 100;
                _currentVolume = (float)value / 100;
                MediaPlayer.Volume = _currentVolume;
            }
        }

        private FadeMode _currentFadeMode;

        public TrackManager()
        {
            TrackCollection = new List<Track>();
            Category = TrackType.Mute;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            _currentFadeMode = FadeMode.In;
            _maxVolume = 1.0f;
            _currentVolume = 0;
            _stepSize = 0.01f;
            MediaPlayer.Volume = _currentVolume;
        }

        private void FadeIn()
        {
            if (Instance._currentVolume < Instance._maxVolume)
                Instance._currentVolume += Instance._stepSize;
            else
            {
                Instance._currentVolume = Instance._maxVolume;
                _currentFadeMode = FadeMode.None;
            }
            try
            {
                MediaPlayer.Volume = Instance._currentVolume;
            }
            catch (Exception ex)
            {

            }
            
        }

        private void FadeOut()
        {
            if (Instance._currentVolume > 0)
                Instance._currentVolume -= Instance._stepSize;
            else
            {
                Instance._currentVolume = 0;
                _currentFadeMode = FadeMode.None;
                MediaPlayer.Stop();
                return;
            }
            MediaPlayer.Volume = Instance._currentVolume;
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (_currentTrackIndexIndex < CategoryTrackIndexes.Count -1)
                    _currentTrackIndexIndex++;
                else
                    _currentTrackIndexIndex = 0;
                MediaPlayer.Stop();
                Play();
            }
        }

        public static void AddTrack(Track toAdd)
        {
            Instance.TrackCollection.Add(toAdd);
        }
        
        public static void Play()
        {
            if (Instance.CategoryTrackIndexes.Count == 0 || MediaPlayer.State == MediaState.Playing)
                return;
            int currentTrackIndex = Instance.CategoryTrackIndexes[Instance._currentTrackIndexIndex];
            Song currentSong = Instance.TrackCollection[currentTrackIndex].Audio;
            MediaPlayer.Volume = Instance._currentVolume;
            Instance._currentFadeMode = FadeMode.In;
            MediaPlayer.Play(currentSong);
        }
        
        private void SortCategoryIndexes()
        {
            switch (Mode)
            {
                case TrackMode.Random:
                    CategoryTrackIndexes.Shuffle();
                    break;
                case TrackMode.Line:
                    CategoryTrackIndexes.Sort();
                    break;
                default:
                    break;
            }
        }
        private void CreateCategoryIndexes()
        {
            CategoryTrackIndexes = new List<int>();
            for (int i = 0; i < TrackCollection.Count; i++)
            {
                if(TrackCollection[i].Category == Category)
                    CategoryTrackIndexes.Add(i);
            }
            SortCategoryIndexes();
            if (MediaPlayer.State == MediaState.Playing)
            {
                _currentTrackIndexIndex = 0;

                _currentFadeMode = FadeMode.Out;

                //MediaPlayer.Stop();
                //Play();
            }

        }

        public static void Update()
        {
            switch (Instance._currentFadeMode)
            {
                case FadeMode.In:
                    Instance.FadeIn();
                    break;
                case FadeMode.Out:
                    Instance.FadeOut();
                    break;
                default:
                    break;
            }
        }
    }
}
