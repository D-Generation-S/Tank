using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Tank.Code.General;
using Tank.Code.GUIClasses;
using Tank.Code.Sound;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class SettingsMain : BasicScreen
    {
        public SettingsMain(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD) : base(ScreenType.Menu, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "gameSetup";
            _guiEvents.Add("Controls_Click", Controls_Click);
            _guiEvents.Add("BackButton_Click", BackButton_Click);
            _guiEvents.Add("FullscreenToggle_Click", FullscreenToggle_Click);
            _guiEvents.Add("MasterVolume_Change", MasterVolume_Change);
            _guiEvents.Add("SplashScreenToggle_Click", SplashScreenToggle_Click);
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            base.ActivateScreen();
            GUISlider masterVolume = _sliders.Where(slider => slider.Name == "Slider_MasterVolume").FirstOrDefault();
            if (masterVolume != null)
                masterVolume.Value = TrackManager.Instance.CurrentVolume;
            GUIToggle fullscreen = _toggles.Where(toggle => toggle.Name == "Toggle_Fullscreen").FirstOrDefault();
            //if (fullscreen != null)
            //fullscreen.Checked = TankGame.GetIsFullscreen();

        }

        private void Controls_Click(object sender, EventArgs e)
        {

        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            BasicScreen MainScreen = new MainScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice);
            ScreenManager.Instance.SetCurrentScreen(MainScreen);
        }

        private void MasterVolume_Change(object sender, EventArgs e)
        {
            int vol = ((GUIEventArgs)e).Value.ToInt();
            Settings.SoundVolume = (float)vol / 100;
            Settings.MusicVolume = (float)vol / 100;
            TrackManager.Instance.CurrentVolume = vol;
            Settings.Save();
        }

        private void FullscreenToggle_Click(object sender, EventArgs e)
        {
            bool isChecked = ((GUIEventArgs)e).Value.ToBool();
            //TankGame.toggleFullscreen(isChecked);
        }

        private void SplashScreenToggle_Click(object sender, EventArgs e)
        {
            bool isChecked = ((GUIEventArgs)e).Value.ToBool();
            Settings.ShowSplashScreen = isChecked;
            Settings.Save();
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }
    }
}
