using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tank.Code.GUIClasses;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class IngameMenuScreen : BasicBackgroundScreen
    {
        public IngameMenuScreen(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD, BasicScreen BackgroundScreen = null) : base(ScreenType.GameOverlay, ScreenWidth, ScreenHeigh, GD, BackgroundScreen)
        {
            Name = "Ingame Overlay";

        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            GUIButton MainMenu = new GUIButton(0, 100, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Main menu", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            GUIButton Back = new GUIButton(0, (int)(MainMenu.Position.Y + MainMenu.Size.Y + 20), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Resume", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            GUIButton ExitGame = new GUIButton(0, (int)(Back.Position.Y + Back.Size.Y + 20), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Back to Desktop", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            Helper.HorizontalCenterGUIElement(MainMenu);
            Helper.HorizontalCenterGUIElement(Back);
            Helper.HorizontalCenterGUIElement(ExitGame);
            MainMenu.Click += MainMenu_Click;
            Back.Click += Back_Click;
            ExitGame.Click += ExitGame_Click;
            base.ActivateScreen(FirstScreen);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetLastScreen();
        }

        private void ExitGame_Click(object sender, EventArgs e)
        {
            Settings.ExitGame = true;
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetCurrentScreen(new MainScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice), true);
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            base.Draw(SB, GD);
        }
    }
}
