using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tank.Interfaces;

namespace Tank.Code.Screenmanager
{
    public class ScreenManager
    {
        public static ScreenManager _instance;
        public static ScreenManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenManager();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private BasicScreen CurrentScreen;
        public BasicScreen ActiveScreen
        {
            get
            {
                return CurrentScreen;
            }
        }

        private BasicScreen LastScreen;

        private List<BasicScreen> OldScreens;

        private bool _firstScreen
        {
            get
            {
                return LastScreen == null ? true : false;
            }
        }

        public bool ExitGame;

        public ScreenManager()
        {
            OldScreens = new List<BasicScreen>();
            ExitGame = false;
        }

        public bool SetLastScreen()
        {
            if (LastScreen == null)
                return false;
            CurrentScreen.DisableScreen();
            CurrentScreen = LastScreen;
            CurrentScreen.ActivateScreen();
            return true;
        }

        public bool SetCurrentScreen(BasicScreen NewScreen, bool DeleteThisScreen = false, bool StoreOldScreen = true)
        {
            for (int i = OldScreens.Count -1; i > 0; i--)
            {
                if (OldScreens[i].IsBackgroundScreen)
                    OldScreens.RemoveAt(i);
            }
            if (String.IsNullOrEmpty(NewScreen.Name))
                return false;
            if (DeleteThisScreen)
            {
                CurrentScreen.Delete = true;
                StoreOldScreen = !DeleteThisScreen;
                LastScreen = null;
            }
            if (CurrentScreen != null)
                LastScreen = CurrentScreen;
            if (LastScreen != null)
                LastScreen.DisableScreen();
            if (StoreOldScreen && LastScreen != null)
            {
                AddOldScreen(LastScreen);
            }
            if (!ScreenAlreadyExist(NewScreen))
            {
                CurrentScreen = NewScreen;
                CurrentScreen.ActivateScreen(_firstScreen);
                return true;
            }
            else
                return ActivateStoredScreen(NewScreen);
        }

        private void AddOldScreen(BasicScreen OldScreen)
        {
            if (!ScreenAlreadyExist(OldScreen))
                OldScreens.Add(OldScreen);
        }

        private bool ActivateStoredScreen(BasicScreen ScreenToActivate)
        {
            bool ReturnVal = false;
            foreach (BasicScreen Screen in OldScreens)
            {
                if (Screen.Name == ScreenToActivate.Name)
                {
                    Screen.ActivateScreen();
                    CurrentScreen = Screen;
                    ReturnVal = true;
                }
            }
            return ReturnVal;
        }

        private bool ScreenAlreadyExist(BasicScreen ScreenToCheck)
        {
            bool ReturnVal = false;
            OldScreens.ForEach(CurrentScreen => 
            {
                if (CurrentScreen.Name == ScreenToCheck.Name)
                {
                    ReturnVal =  true;
                }

            });
            return ReturnVal;
        }

        public void Draw(SpriteBatch sb, GraphicsDevice GD)
        {
            if (CurrentScreen != null)
                CurrentScreen.Draw(sb, GD);
        }

        public void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameIsActive)
        {
            if (CurrentScreen != null)
                CurrentScreen.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameIsActive);
        }

    }
}
