using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Code.GUIClasses;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class RoundEnd : BasicBackgroundScreen
    {
        private List<Player> _playerList;
        private string FontText = "{0} wins with a score of: {1}";
        private bool DrawString;
        private Vector2 StringPostion;

        public RoundEnd(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD, List<Player> CurrentplayerList, Texture2D CustomMap = null, BasicScreen _backgroundScreen = null) : base(ScreenType.GameOverlay, ScreenWidth, ScreenHeigh, GD, _backgroundScreen)
        {
            Name = "RoundEnd";
            _playerList = CurrentplayerList;
            DrawString = false;
            CreateText();
            
        }


        private void CreateText()
        {
            Player CurrentPlayer = null;
            foreach (Player p in _playerList)
            {
                if (p.IsAlive)
                {
                    CurrentPlayer = p;
                    break;
                }
            }
            if (CurrentPlayer != null)
            {
                DrawString = true;
                if (!String.IsNullOrEmpty(CurrentPlayer.PlayerName))
                    FontText = String.Format(FontText, CurrentPlayer.PlayerName, CurrentPlayer.Money);
            }
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            GUIButton MainMenu = new GUIButton(0, 100, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Main menu", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            GUIButton PlayAgain = new GUIButton(0, (int)(MainMenu.Position.Y + MainMenu.Size.Y + 20), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Play again", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            GUIButton ExitGame = new GUIButton(0, (int)(PlayAgain.Position.Y + PlayAgain.Size.Y + 20), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Back to Desktop", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound, true);
            Helper.HorizontalCenterGUIElement(MainMenu);
            StringPostion = new Vector2(MainMenu.Position.X + MainMenu.Size.X / 2, MainMenu.Position.Y);
            Helper.HorizontalCenterGUIElement(PlayAgain);
            Helper.HorizontalCenterGUIElement(ExitGame);
            MainMenu.Click += MainMenu_Click;
            PlayAgain.Click += Back_PlayAgain;
            PlayAgain.Hidden = true;
            ExitGame.Click += ExitGame_Click;

            StringPostion = new Vector2(StringPostion.X - (Settings.GlobalFont.MeasureString(FontText).X / 2), StringPostion.Y - (Settings.GlobalFont.MeasureString(FontText).Y + 20));

            base.ActivateScreen(FirstScreen);
        }

        private void ExitGame_Click(object sender, EventArgs e)
        {
            Settings.ExitGame = true;
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }

        private void Back_PlayAgain(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetCurrentScreen(new GameScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, _playerList), true);
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetCurrentScreen(new MainScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice), true);
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            base.Draw(SB, GD);
            if (DrawString)
                SB.DrawString(Settings.GlobalFont, FontText, StringPostion, Color.Black);
        }

        public override void DisableScreen()
        {
            ActiveHandler.Instance.EndRound();
            base.DisableScreen();
        }
    }
}
