using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tank.Code.JSonClasses;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    internal class SplashScreen : BasicScreen
    {
        private int _currentSplashIndex = 0;
        private int elapsedTime = 0;
        private GameTime PreviousGameTime;

        private SplashObject _currentSplashObject
        {
            get
            {
                if (_currentSplashIndex < SplashList.Count)
                    return SplashList[_currentSplashIndex];
                else
                    return null;
            }
        }

        private Texture2D _currentSplashTexture;

        public List<SplashObject> SplashList
        {
            get;
            set;
        }

        public SplashScreen() : base()
        {
            Name = "Splash Screen";
            TrackType = TrackType.Splash;
        }

        public SplashScreen(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD) : base(ScreenType.Menu, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "Splash Screen";
            TrackType = TrackType.Splash;
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            base.ActivateScreen(FirstScreen);
        }


        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            if (PreviousGameTime == null)
                PreviousGameTime = CurrentGameTime;
            elapsedTime += CurrentGameTime.ElapsedGameTime.Milliseconds;

            if (_currentSplashObject == null || !Settings.ShowSplashScreen)
                MoveOn();


            if (elapsedTime <= _currentSplashObject.Duration)
            {
                if (_currentSplashTexture == null)
                    _currentSplashTexture = TankGame.PublicContentManager.Load<Texture2D>(_currentSplashObject.Image);
            }
            else
            {
                elapsedTime = 0;
                _currentSplashTexture = null;
                _currentSplashIndex++;
            }
            if (Controls.KeyToogle(Keys.Space))
            {
                elapsedTime = 0;
                _currentSplashTexture = null;
                _currentSplashIndex++;
            }

            if (_currentSplashIndex == SplashList.Count)
                MoveOn();

            PreviousGameTime = CurrentGameTime;

            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            base.Draw(SB, GD);
            if (_currentSplashTexture != null)
            {
                GD.Clear(FillColor);
                int x = 40;
                int y = 40;

                int maxWidth = Settings.MaxWindowSize.Width - 80;
                int maxHeight = Settings.MaxWindowSize.Height - 80;

                int width = _currentSplashTexture.Width;
                int height = _currentSplashTexture.Height;

                if (width < maxWidth)
                    x = (Settings.MaxWindowSize.Width / 2) - (_currentSplashTexture.Width / 2);
                else
                {
                    width = maxWidth;
                    height = ResizeRatio(_currentSplashTexture, maxWidth);
                }

                if (height < maxHeight)
                    y = (Settings.MaxWindowSize.Height / 2) - (_currentSplashTexture.Height / 2);
                else
                    height = maxHeight;

                SB.Draw(_currentSplashTexture, new Rectangle(x, y, width, height), Color.White);
            }
        }

        private int ResizeRatio(Texture2D _currentSplashTexture, int maxWidth)
        {
            int newHeight = 0;
            double currentRatio = (double)_currentSplashTexture.Height / (double)_currentSplashTexture.Width;
            newHeight = (int)(currentRatio * maxWidth);
            return newHeight;
        }

        private void MoveOn()
        {
            BasicScreen MainMenu = new MainScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, TankGame.PublicGraphicsDevice);
            MainMenu.BackgroundImage = TankGame.PublicContentManager.Load<Texture2D>("Images/Backgrounds/Tank_Background_Distort_Shadow");
            ScreenManager.Instance.SetCurrentScreen(MainMenu);
        }
    }
}