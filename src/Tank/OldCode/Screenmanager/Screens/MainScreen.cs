using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Tank.Code.General;
using Tank.Code.GUIClasses;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{

    internal class MainScreen : BasicScreen
    {
        public MainScreen(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD) : base(ScreenType.Menu, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "mainMenu";
            _guiEvents.Add("StartGame_Click", B_Click);
            _guiEvents.Add("MapEditor_Click", MapEditor_Click);
            _guiEvents.Add("GameSettings_Click", GameSettings_Click);
            _guiEvents.Add("ExitGame_Click", ExitGame_Click);
            _guiEvents.Add("Link_Click", Link_Click);
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            base.ActivateScreen(FirstScreen);
        }

        private void MapEditor_Click(object sender, EventArgs e)
        {
            BasicScreen _mapEditor = new MapEditorScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice);
            ScreenManager.Instance.SetCurrentScreen(_mapEditor);
        }

        private void GameSettings_Click(object sender, EventArgs e)
        {
            BasicScreen SettingsMain = new SettingsMain(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice);
            ScreenManager.Instance.SetCurrentScreen(SettingsMain);
        }

        private void ExitGame_Click(object sender, EventArgs e)
        {
            Settings.ExitGame = true;
        }

        private void B_Click(object sender, EventArgs e)
        {
            //BasicScreen Game = new GameScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, Helper.LoadCustomMap("MyMap", _graphicDevice));
            BasicScreen Lobby = new LobbyScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice);
            Lobby.BackgroundImage = TankGame.PublicContentManager.Load<Texture2D>("Images/Backgrounds/Tank_Background_Distort_Shadow");

            ScreenManager.Instance.SetCurrentScreen(Lobby);
        }

        private void Link_Click(object sender, EventArgs e)
        {
            GUIButton btn = (GUIButton)sender;
            string url = btn.InternalData.ToSafeString();
            if (!string.IsNullOrEmpty(url))
                Process.Start(url);
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }
    }
}