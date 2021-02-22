using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Tank.Code.GUIClasses;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    public enum Pencil
    {
        Square,
        Circle,
    }



    class MapEditorScreen : BasicScreen
    {
        private int _screenWidth;
        private int _screenHeight;

        //Brushes
        private GUIButton _extendBrushes;
        private GUIButton _brushSquare;
        private GUIButton _brushCircle;

        private string _baseSwitchMaterialText = "Current: ";
        private string _baseSwitchOverrideText = "Override: ";
        private Settings.ClimaticZones _currenClimaZone;
        private GUIButton _switchMaterial;
        private GUIButton _switchOverride;
        private GUIButton _save;

        private Texture2D _currentBrush;
        private Vector2 _mousePosition;


        private Keys _drawKey;
        private KeyboardState _lastState;

        private Pencil _currentPencil;
        private int _pencilSize;
        private bool _drawMode;

        private Color _currentColor;

        private int _lastScrollValue;
        private int _scrollStep;

        private bool _override;

        public MapEditorScreen(int ScreenWidth, int ScreenHeight, GraphicsDevice GD) : base(ScreenType.Game, ScreenWidth, ScreenHeight, GD)
        {
            Name = "MapEditor";
            _currenClimaZone = Settings.ClimaticZones.Dirt;
            _drawKey = Keys.D;
            _graphicDevice = GD;
            _fillBackGround = false;

            _screenWidth = ScreenWidth;
            _screenHeight = ScreenHeight;
            CreateEmptyMap();

            _currentPencil = Pencil.Square;

            _drawMode = false;
            _currentColor = Settings.GetColorByClimaticZone(_currenClimaZone);

            _pencilSize = 5;
            ForceDrawCursor = !_drawMode;
            _currentBrush = CreateBrushTexture(_pencilSize);
            _scrollStep = 1;
            _override = false;
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            _extendBrushes = new GUIButton(0, _screenHeight - Settings.ProgressBackground.Height - 20, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Show brushes", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _extendBrushes.HorizontalCenterGUIElement();
            _extendBrushes.InternalData = false;

            _extendBrushes.Click += _extendBrushes_Click;



            _brushSquare = new GUIButton((int)_extendBrushes.Position.X - (int)(_extendBrushes.Size.X / 2) - 10, _screenHeight - (40 + (int)_extendBrushes.Size.Y + Settings.ProgressBackground.Height), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Square", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _brushSquare.Click += _brushSquare_Click;
            _brushSquare.Hidden = true;
            _brushCircle = new GUIButton((int)_extendBrushes.Position.X + (int)(_extendBrushes.Size.X / 2) + 10, _screenHeight - (40 + (int)_extendBrushes.Size.Y + Settings.ProgressBackground.Height), Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Circle", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _brushCircle.Click += _brushCircle_Click;
            _brushCircle.Hidden = true;

            _switchMaterial = new GUIButton((int)_brushSquare.Position.X, 0 + Settings.ProgressBackground.Height, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Show brushes", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _switchMaterial.Text = $"{_baseSwitchMaterialText} {_currenClimaZone.ToString()}";
            _switchMaterial.Click += _switchMaterial_Click;

            _switchOverride = new GUIButton((int)_brushCircle.Position.X, 0 + Settings.ProgressBackground.Height, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Show brushes", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _switchOverride.Text = $"{_baseSwitchOverrideText} {_override.ToString()}";
            _switchOverride.Click += _switchOverride_Click;



            _save = new GUIButton(_screenWidth - Settings.ProgressBackground.Width - 20, _screenHeight - Settings.ProgressBackground.Height - 20, Settings.ProgressBackground.Width, Settings.ProgressBackground.Height, _graphicDevice, Settings.ProgressBackground, "Show brushes", Settings.GlobalFont, Settings.ProgressForeground, Settings.ClickSound);
            _save.Text = "Save";
            _save.Click += _save_Click;
            base.ActivateScreen(FirstScreen);
        }

        private void _save_Click(object sender, EventArgs e)
        {
            Save($"{DateTime.Now.ToString("yyyyMMddHmmss")}.png", false);
        }

        private void _switchOverride_Click(object sender, EventArgs e)
        {
            _override = !_override;
            _switchOverride.Text = $"{_baseSwitchOverrideText} {_override.ToString()}";
        }

        private void _switchMaterial_Click(object sender, EventArgs e)
        {
            int CurrentNumber = (int)_currenClimaZone;
            CurrentNumber++;
            Array Numbers = Enum.GetValues(typeof(Settings.ClimaticZones));
            if (CurrentNumber >= Numbers.Length)
                CurrentNumber = 0;

            _currenClimaZone = (Settings.ClimaticZones)CurrentNumber;

            _switchMaterial.Text = $"{_baseSwitchMaterialText} {_currenClimaZone.ToString()}";
            _currentColor = Settings.GetColorByClimaticZone(_currenClimaZone);
        }

        private void _brushCircle_Click(object sender, EventArgs e)
        {
            _currentPencil = Pencil.Circle;
            _currentBrush = CreateBrushTexture(_pencilSize);
        }

        private void _brushSquare_Click(object sender, EventArgs e)
        {
            _currentPencil = Pencil.Square;
            _currentBrush = CreateBrushTexture(_pencilSize);
        }

        private void _extendBrushes_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(GUIButton))
            {
                GUIButton CurrentButton = (GUIButton)sender;
                if ((bool)CurrentButton.InternalData)
                {
                    _brushSquare.Hidden = true;
                    _brushCircle.Hidden = true;
                    CurrentButton.Text = "Show brushes";
                }
                else
                {
                    _brushSquare.Hidden = false;
                    _brushCircle.Hidden = false;
                    CurrentButton.Text = "Hide brushes";
                }
                CurrentButton.InternalData = !(bool)CurrentButton.InternalData;
            }
        }

        private void CreateEmptyMap()
        {
            Texture2D Picture = new Texture2D(_graphicDevice, _screenWidth, _screenHeight);
            Color[] colorData = new Color[_screenWidth * _screenHeight];

            for (int y = 0; y < _screenHeight; y++)
            {
                for (int x = 0; x < _screenWidth; x++)
                {
                    int Index = x + y * _screenWidth;
                    colorData[Index] = Color.Black;
                }
            }
            Terrain.Instance.Initialize(Picture, 1);

        }

        private bool Save(string Name, bool Override)
        {
            if (!Directory.Exists(Settings.CustomMapFolder))
                Directory.CreateDirectory(Settings.CustomMapFolder);

            Name = Settings.CustomMapFolder + Name;
            if (File.Exists(Name) && !Override)
                return false;

            using (StreamWriter writer = new StreamWriter(Name))
            {
                Terrain.Instance.CurrentMap.SaveAsPng(writer.BaseStream, Terrain.Instance.CurrentMap.Width, Terrain.Instance.CurrentMap.Height);
            }
            return true;
        }

        private void SwitchGUIElementState(bool NewValue)
        {
            _extendBrushes.Hidden = NewValue;
            _switchMaterial.Hidden = NewValue;
            _switchOverride.Hidden = NewValue;
            _save.Hidden = NewValue;
            if (NewValue)
            {
                _brushCircle.Hidden = NewValue;
                _brushSquare.Hidden = NewValue;
            }
            else
            {
                _brushCircle.Hidden = !(bool)_extendBrushes.InternalData;
                _brushSquare.Hidden = !(bool)_extendBrushes.InternalData;
            }

        }

        private Texture2D CreateBrushTexture(int Size)
        {
            if (_currentPencil == Pencil.Square)
                return CreateSquare(Size);
            if (_currentPencil == Pencil.Circle)
                return CreateCircle(Size);
            return null;
        }

        private Texture2D CreateSquare(int Size)
        {
            Size = Size * 2 + 1;
            Texture2D Picture = new Texture2D(_graphicDevice, Size, Size);
            Color[] colorData = new Color[Size * Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    int Index = x + y * Size;
                    if (x == 0 || x == Size - 1)
                        colorData[Index] = Color.Black;
                    if (y == 0 || y == Size - 1)
                        colorData[Index] = Color.Black;


                }
            }
            Picture.SetData<Color>(colorData);
            return Picture;
        }

        private Texture2D CreateCircle(int Size)
        {
            int Radius = Size;
            int InnerRadius = Size - 1;
            Size = Size * 2 + 1;
            Vector2 _circlePos = new Vector2(Size / 2, Size / 2);
            Texture2D Picture = new Texture2D(_graphicDevice, Size, Size);
            Color[] colorData = new Color[Size * Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    int Index = x + y * Size;
                    if (PointInCircle((int)_circlePos.X, (int)_circlePos.Y, x, y, Radius))
                        colorData[Index] = Color.Black;


                }
            }

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    int Index = x + y * Size;
                    if (PointInCircle((int)_circlePos.X, (int)_circlePos.Y, x, y, InnerRadius))
                        colorData[Index] = Color.Transparent;


                }
            }

            Picture.SetData<Color>(colorData);
            return Picture;
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            Terrain.Instance.Update();
            _mousePosition = new Vector2(MouseHandler.Position.X - _currentBrush.Width / 2, MouseHandler.Position.Y - _currentBrush.Height / 2);
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            if (GameActive)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    ScreenManager.Instance.SetCurrentScreen(new IngameMenuScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, this));
                }

                if (CurrentKeyboardState.IsKeyDown(_drawKey) && _lastState.IsKeyUp(_drawKey))
                {
                    SwitchGUIElementState(!_drawMode);
                    _drawMode = !_drawMode;
                    ForceDrawCursor = !_drawMode;
                }

                if (_drawMode)
                {
                    if (MouseHandler.LeftButton == ButtonState.Pressed)
                    {
                        if (_currentPencil == Pencil.Square)
                            DrawSquare(CurrentMouseState.X, CurrentMouseState.Y, _pencilSize);
                        if (_currentPencil == Pencil.Circle)
                            DrawCircle(CurrentMouseState.X, CurrentMouseState.Y, _pencilSize);
                    }
                    if (CurrentMouseState.RightButton == ButtonState.Pressed)
                    {
                        if (_currentPencil == Pencil.Square)
                            DrawSquare(CurrentMouseState.X, CurrentMouseState.Y, _pencilSize, true);
                        if (_currentPencil == Pencil.Circle)
                            DrawCircle(CurrentMouseState.X, CurrentMouseState.Y, _pencilSize, true);
                    }
                    ;
                    if (CurrentMouseState.ScrollWheelValue != _lastScrollValue)
                    {

                        int NewValue = _pencilSize;
                        if (CurrentMouseState.ScrollWheelValue < _lastScrollValue)
                            NewValue -= _scrollStep;
                        else if (CurrentMouseState.ScrollWheelValue > _lastScrollValue)
                            NewValue += _scrollStep;
                        if (NewValue <= 0)
                            NewValue = 1;
                        _currentBrush = CreateBrushTexture(NewValue);
                        _pencilSize = NewValue;
                    }



                }
            }
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
            _lastState = CurrentKeyboardState;
            _lastScrollValue = CurrentMouseState.ScrollWheelValue;
        }

        private void DrawSquare(int x, int y, int Size, bool Remove = false)
        {
            if (Size == 0)
                return;
            for (int i = -Size; i <= Size; i++)
                for (int j = -Size; j <= Size; j++)
                    if (Remove)
                        Terrain.RemovePixel(x + i, y + j);
                    else
                        Terrain.AddPixel(_currentColor, x + i, y + j, _override);
        }

        private void DrawCircle(int x, int y, int radius, bool Remove = false)
        {
            if (radius == 0)
                return;
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (PointInCircle(x + i, y + j, x, y, radius))
                    {
                        if (Remove)
                            Terrain.RemovePixel(x + i, y + j);
                        else
                            Terrain.AddPixel(_currentColor, x + i, y + j, _override);
                    }
                }
            }
        }

        private bool PointInCircle(int Cx, int Cy, int Px, int Py, int Radius)
        {
            float TestValue = (float)Math.Sqrt(Math.Pow((Px - Cx), 2) + Math.Pow((Py - Cy), 2));
            if (TestValue < Radius)
                return true;
            return false;
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            Terrain.Instance.Draw(SB, 0, 0);
            if (!_drawMode)
            {
                SB.DrawString(Settings.GlobalFont, $"Drawing disabled press {_drawKey.ToString()} to enable drawing", new Vector2(0, 0), Color.Red);
            }
            else
                SB.DrawString(Settings.GlobalFont, $"Drawing enabled press {_drawKey.ToString()} to disable drawing", new Vector2(0, 0), Color.Black);

            if (_currentBrush != null && _drawMode)
                SB.Draw(_currentBrush, _mousePosition, Color.White);
            base.Draw(SB, GD);
        }
    }
}
