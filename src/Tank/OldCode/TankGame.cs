using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tank.Code;
using Tank.Code.General;
using Tank.Code.GUIClasses;
using Tank.Code.JSonClasses;
using Tank.Code.Screenmanager;
using Tank.Code.Screenmanager.ViewPortAdapters;
using Tank.Code.Sound;
using Tank.Code.TerrainClasses;
using Tank.Enums;

namespace Tank.OldCode
{
    public class TankGame : Game
    {
        private static TankGame Instance;
        private FrameCounter _frameCounter;
        private Terrain map;
        private ImageGenerator MapGenerator;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //KeyboardState ksPrevious;
        private MouseState LastState;

        private List<Player> player;
        private Cloud c;
        public static GraphicsDevice PublicGraphicsDevice;
        public static ContentManager PublicContentManager;

        private BoxingViewportAdapter _adapter;
        private Keys[] lastKeys;
        private object _mousePosition;

        private bool bNeedsColor = false;
        private string strColorValues = "Coords: {0}x{1} - Solid: {2} - Color: R({3})G({4})B({5})";

        public TankGame()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            PublicGraphicsDevice = GraphicsDevice;
            PublicContentManager = Content;
            Settings.ExitGame = false;
            Settings.Debug = false;
            Settings.Drawing = false;
            Settings.ExplodingClicks = false;
            Settings.ShowRayCast = false;
            Settings.PlayerMovingAllowed = false;

            Settings.Meter = Content.Load<Texture2D>("Images/GUI/meter_big");
            Settings.Bar = Content.Load<Texture2D>("Images/GUI/meter_bar");
            Settings.ProgressBackground = Content.Load<Texture2D>("Images/GUI/ProgressBackground");
            Settings.ProgressForeground = Content.Load<Texture2D>("Images/GUI/ProgressFront");
            Settings.PlayerAngleBackground = Content.Load<Texture2D>("Images/GUI/AngleContainer");
            Settings.PlayerAngleForeground = Content.Load<Texture2D>("Images/GUI/AngleDisplay");
            Settings.GlobalFont = Content.Load<SpriteFont>("gameFont");
            Settings.DeathSound = Content.Load<SoundEffect>("Audio/Sounds/Basic_Death");
            Settings.DebugTextColor = Color.Black;


            Settings.MaxWindowSize = new Rectangle(0, 0, 1920, 1080);
            InitResolution(1440, 900);

            TextureTerrain();

            Settings.ScreenshotFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\TankGame";
            Settings.SaveFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TankGame\\Settings.xml";

            Settings.GoLeftKey = new Keys[] { Keys.A, Keys.Left };
            Settings.GoRightKey = new Keys[] { Keys.D, Keys.Right };
            Settings.ShootKey = new Keys[] { Keys.Space, Keys.Enter };
            Settings.IncreasePower = new Keys[] { Keys.W, Keys.Up };
            Settings.DecreasePower = new Keys[] { Keys.S, Keys.Down };

            Settings.Left = Buttons.DPadLeft;
            Settings.Right = Buttons.DPadRight;
            Settings.Shoot = Buttons.A;
            Settings.IncPower = Buttons.DPadUp;
            Settings.DecPower = Buttons.DPadDown;
            Settings.DynamicPixelTexture = Content.Load<Texture2D>("Pixel");
            Settings.GameCursor = Content.Load<Texture2D>("Images/GUI/Cursor-icon");
            Settings.ClickSound = Content.Load<SoundEffect>("Audio/Sounds/ButtonClick");

            Settings.ShootSound = Content.Load<SoundEffect>("Audio/Sounds/Basic_Shoot");
            Settings.ExplosionSound = Content.Load<SoundEffect>("Audio/Sounds/Basic_Explosion");
            Settings.HitSound = Content.Load<SoundEffect>("Audio/Sounds/Basic_Hit");

            _frameCounter = new FrameCounter();


            TrackManager.AddTrack(new Track(Content.Load<Song>("Audio\\Tracks\\TankBattleMusic"), TrackType.Splash));
            TrackManager.Instance.Category = TrackType.Menu;
            TrackManager.Instance.Mode = TrackMode.Random;

            string splashJson = CodeHelper.LoadJson("GUI\\splash");

            ScreenDefinition splashDefinition = JsonConvert.DeserializeObject<ScreenDefinition>(splashJson);
            SplashScreen splashSreen = new SplashScreen();
            splashSreen.SplashList = splashDefinition.SplashList;
            splashSreen.FillColor = CodeHelper.ConvertFromHex(splashDefinition.BackgroundColor);
            ScreenManager.Instance.SetCurrentScreen(splashSreen);

            Settings.Load();

            base.Initialize();
        }

        private void InitResolution(int windowWidth, int windowHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;

            _adapter = new BoxingViewportAdapter(Window, graphics, Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, 88, 70);

            graphics.IsFullScreen = false;

            graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Settings.BasicMunitionSprite = Content.Load<Texture2D>("Images/Assets/BasicMunitionSprite");
        }

        private void TestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.GetType() == typeof(Texture2D))
                Terrain.Instance.Initialize((Texture2D)e.Result, 3);
        }

        private void TextureTerrain(string texture = "Images/Textures/groundTile", string stoneTexture = "Images/Textures/rocktile")
        {
            Texture2D t2D = null;
            Texture2D t2DRock = null;
            try
            {
                t2D = Content.Load<Texture2D>(texture);
                t2DRock = Content.Load<Texture2D>(stoneTexture);
            }
            catch
            {
                Settings.ColorTextureAssignments.Clear();
            }
            Helper.AddColorTextureAssignment(Color.Bisque, t2D);
            Helper.AddColorTextureAssignment(Color.SaddleBrown, t2D);
            Helper.AddColorTextureAssignment(Color.Green, t2D);
            Helper.AddColorTextureAssignment(Color.White, t2D);
            Helper.AddColorTextureAssignment(Color.DarkGray, t2DRock);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseHandler.Update(Mouse.GetState(), _adapter);
            Settings.GameTime = gameTime;
            if (Settings.ExitGame)
                Exit();
            Settings.CurrentKeyboardState = Keyboard.GetState();
            if (IsActive)
            {
                if (Controls.KeyToogle(Keys.F12))
                {
                    Settings.Debug = !Settings.Debug;
                    Settings.Drawing &= Settings.Debug;
                }
                HandleControls();

                if (bNeedsColor)
                {
                    for (int i = 0; i < 834; i++)
                    {
                        for (int j = 0; j < 502; j++)
                        {
                            Terrain.RemovePixel(i, j);
                        }
                    }
                    bNeedsColor = false;
                }


                if (Settings.Drawing && IsActive)
                {
                    HandleDrawing();
                }


                LastState = Mouse.GetState();
                TrackManager.Update();
                base.Update(gameTime);
                ScreenManager.Instance.Update(Mouse.GetState(), Keyboard.GetState(), gameTime, GamePad.GetState(PlayerIndex.One), IsActive);
                Settings.PreviousKeyboardState = Keyboard.GetState();
                lastKeys = Keyboard.GetState().GetPressedKeys();
            }
        }

        private void HandleControls()
        {
            if (Settings.Debug)
            {
                if (Controls.KeyToogle(Keys.F6))
                {
                    Settings.ShowRayCast = !Settings.ShowRayCast;
                }
            }

            //if (Controls.KeyToogle(Keys.F9))
            //{
            //    Helper.TakeScreenshot(CompletetGameScreen);
            //}
        }

        private void HandleDrawing()
        {
            Vector2 v2MousePosTemp = Mouse.GetState().Position.ToVector2();
            float mouseX = MathHelper.Clamp(v2MousePosTemp.X, 0, Settings.MaxWindowSize.Width);
            float mouseY = MathHelper.Clamp(v2MousePosTemp.Y, 0, Settings.MaxWindowSize.Height);
            if (Mouse.GetState().RightButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = -5; i <= 5; i++)
                    for (int j = -5; j <= 5; j++)
                        Terrain.AddPixel(Color.Red, mouseX + i, mouseY + j);
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed)// && LastState.LeftButton == ButtonState.Released)
            {
                if (Settings.ExplodingClicks)
                {
                    Explode E = new Explode((int)mouseX, (int)mouseY, 25);
                }
                else
                {
                    if (Mouse.GetState().Position.X > 0 && Mouse.GetState().Position.X < Settings.MaxWindowSize.Width &&
                        Mouse.GetState().Position.Y > 0 && Mouse.GetState().Position.Y < Settings.MaxWindowSize.Height)
                    {
                        float speed = 800 * (1 - 0.001f / 250);
                        float velX = speed * (new Random().Next(-10, 10));// / 0.001f);
                        float velY = speed * (new Random().Next(-10, 10));// / 0.001f);

                        DynamicPixel dm = new DynamicPixel(Color.Red, mouseX, mouseY, velX, velY, 1, true);
                        Physics.Instance.Add(dm);
                        Renderer.Instance.Add(dm);
                    }
                }
                //Terrain.AddPixel(Color.Black, mouseX, mouseY);
            }
            else if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                Terrain.RemovePixel(mouseX, mouseY);
            }
        }

        private void RegenerateTerrain()
        {
            Physics.Instance.RemoveAllPhysicPixels();
            Renderer.Instance.RemoveAllRenderPixels();
            MapGenerator = new ImageGenerator(GraphicsDevice, Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, Settings.MaxWindowSize.Height / 4, 0.5f, true);
            BackgroundWorker TestWorker = MapGenerator.GenerateImageAsync();
            TestWorker.RunWorkerCompleted += TestWorker_RunWorkerCompleted;
            //Terrain.Instance.Initialize(MapGenerator.GenerateImage(), 5);

            //player.ForEach(pTemp =>
            //{
            //    pTemp.Move((float)new Random(player.IndexOf(pTemp)).NextDouble() * CompletetGameScreen.Width, 20);
            //});
        }

        private void AddGuiTextBox()
        {
            GUITextBox tb = new GUITextBox(200, 200, 200, 12, Content.Load<Texture2D>("Pixel"), Content.Load<Texture2D>("Pixel_Red"));
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, _adapter.GetScaleMatrix());


            ScreenManager.Instance.Draw(spriteBatch, GraphicsDevice);
            GraphicsDevice.SetRenderTarget(null);

            //spriteBatch.Draw(CompletetGameScreen, new Rectangle(0, 0, CompletetGameScreen.Width, CompletetGameScreen.Height), Color.White);

            if (Settings.Debug)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _frameCounter.Update(deltaTime);

                StringBuilder strbDebugText = new StringBuilder("FPS: ");
                strbDebugText.Append(_frameCounter.AverageFramesPerSecond.ToString("00.00"));
                strbDebugText.Append("  [F11=Drawing][F10=Reload][F9=Screenshot]");
                strbDebugText.Append("\n\r");
                strbDebugText.Append(string.Format("Current physic objects: {0}", Physics.Instance.objects.Count));
                strbDebugText.Append("\n\r");
                strbDebugText.Append(string.Format("Current drawn objects: {0}", Renderer.Instance.objects.Count));
                strbDebugText.Append("\n\r");
                strbDebugText.Append("[F11] - Drawing: ");
                strbDebugText.Append(Settings.Drawing);
                if (Settings.ColorTextureAssignments.Count <= 0)
                {
                    strbDebugText.Append("\n\r");
                    strbDebugText.Append("Texturize Terrain: ");
                    strbDebugText.Append("false");
                }
                strbDebugText.Append("\n\r");
                strbDebugText.Append("[F10] - Explode: ");
                strbDebugText.Append(Settings.ExplodingClicks);
                strbDebugText.Append("\n\r");
                strbDebugText.Append("[F06] - Show Raycast: ");
                strbDebugText.Append(Settings.ShowRayCast);
                strbDebugText.Append("\n\r");

                foreach (Keys key in lastKeys)
                {
                    strbDebugText.Append(key.ToString());
                }

                spriteBatch.DrawString(Settings.GlobalFont, strbDebugText.ToString(), new Vector2(1, 1), Settings.DebugTextColor, 0, new Vector2(0, 0), 0.9f, SpriteEffects.None, 1);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void toggleFullscreen(bool fullscreen)
        {
            if (Instance.graphics.IsFullScreen != fullscreen)
            {
                Instance.graphics.IsFullScreen = fullscreen;
                Instance.graphics.ApplyChanges();
            }
        }

        public static bool GetIsFullscreen()
        {
            return Instance.graphics.IsFullScreen;
        }
    }
}