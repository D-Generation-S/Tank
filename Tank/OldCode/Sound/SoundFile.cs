using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank.Code.Sound
{
    public class SoundFile
    {
        private string _titleName;
        public string TitleName
        {
            get
            {
                return _titleName;
            }
        }
        private string _authorName;
        public string AuthorName
        {
            get
            {
                return _authorName;
            }
        }

        private SoundEffect _currentTitle;
        private SoundEffectInstance _currentInstance;
        public TimeSpan Duration
        {
            get
            {
                return _currentTitle.Duration;
            }
        }

        public event EventHandler Finished;

        public SoundFile(SoundEffect SoundFile, string Title = "Unknown", string AuthorName = "Unknown")
        {
            _currentTitle = SoundFile;
            _currentInstance = _currentTitle.CreateInstance();
        }
        public void SetSoundInfo(float Volume)
        {
            _currentInstance.Volume = Volume;
        }

        public void Play()
        {
            if (_currentInstance != null && _currentInstance.State != SoundState.Playing)
                _currentInstance.Play();
        }

        public void PauseStart()
        {
            if (_currentInstance != null)
                if (_currentInstance.State != SoundState.Paused && _currentInstance.State != SoundState.Stopped)
                    _currentInstance.Pause();
                else if ((_currentInstance.State == SoundState.Paused && _currentInstance.State != SoundState.Stopped))
                    _currentInstance.Resume();
        }


        public void Stop()
        {
            if (_currentInstance != null && _currentInstance.State != SoundState.Playing)
            {
                _currentInstance.Stop();
            }
        }

        private void TriggerFinishedEvent()
        {
            Finished?.Invoke(this, null);
        }
    }
}
