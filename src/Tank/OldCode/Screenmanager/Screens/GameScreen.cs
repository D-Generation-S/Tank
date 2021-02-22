using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class GameScreen : BasicScreen
    {
        private ImageGenerator MapGenerator;
        private List<Player> _initPlayers;
        private List<Player> _players;
        public List<Player> Players
        {
            get
            {
                return _initPlayers;
            }
        }

        private bool RoundEnd;

        private float WaitTimeToEndGame;
        private float CurrentTime;

        private KeyboardState LastKeyboardState;

        public GameScreen(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD, List<Player> Players, Texture2D CustomMap = null) : base(ScreenType.Game, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "Gamescreen";
            _fillBackGround = false;
            MapGenerator = new ImageGenerator(GD, Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, Settings.MaxWindowSize.Height / 4, 0.5f, true);
            if (CustomMap == null)
                Terrain.Instance.Initialize(MapGenerator.GenerateImage(), 3);
            else
                Terrain.Instance.Initialize(CustomMap, 3);
            _players = Players;
            _initPlayers = new List<Player>();
            _players.ForEach(player =>
            {
                _initPlayers.Add(player);
            });
            RoundEnd = false;

            WaitTimeToEndGame = 2000.0f;
            CurrentTime = 0f;
        }

        private void SpawnPlayer()
        {
            int zoneWidth = Settings.MaxWindowSize.Width / _players.Count - 64;
            bool FirstPlayer = true;
            Random posRand = new Random(MapGenerator.Seed);
            _players.ForEach(player =>
            {
                player.Active = FirstPlayer;
                FirstPlayer = false;
                player.LoadContent(Settings.Meter, Settings.Bar, Settings.ProgressBackground, Settings.ProgressForeground);
                player.Initialize(zoneWidth, posRand);
            });
            ActiveHandler.Instance.InitializeNewRound();
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            GD.Clear(Color.CornflowerBlue);
            Terrain.Instance.Draw(SB, 0, 0);
            base.Draw(SB, GD);
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            if (CurrentKeyboardState.IsKeyUp(Keys.Escape) && LastKeyboardState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.Instance.SetCurrentScreen(new IngameMenuScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, this));
            }

            if (RoundEnd)
            {
                if (CurrentTime >= WaitTimeToEndGame)
                {
                    ScreenManager.Instance.SetCurrentScreen(new RoundEnd(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, _players, null, this), false);
                }
                CurrentTime += 1 * (float)CurrentGameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (!GameActive)
            {
                Settings.PlayerCurrentDisabled = true;
            }
            Terrain.Instance.Update();
            Physics.Instance.Update(CurrentGameTime);
            ActiveHandler.Instance.Update(Mouse.GetState(), Keyboard.GetState(), CurrentGameTime, GamePad.GetState(ActiveHandler.Instance.CurrentPlayer));

            if (PlayersAlive() <= 1)
            {
                RoundEnd = true;
            }
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
            LastKeyboardState = CurrentKeyboardState;
        }

        private int PlayersAlive()
        {
            int PlayerAlive = 0;
            _players.ForEach(pTemp =>
            {
                if (pTemp.IsAlive)
                {
                    PlayerAlive++;
                }
            });
            return PlayerAlive;
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            if (_firstCall)
                SpawnPlayer();
            base.ActivateScreen(FirstScreen);

        }
    }
}
