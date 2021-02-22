using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Fluids : IActiveObj, IDisposable
    {

        private Texture2D _currentTexture;
        private Color[] _colorData;

        private byte[] _mass;
        private byte[] _newmass;
        private int _realMapWidth;
        private int _mapWidht
        {
            get
            {
                return _realMapWidth / (PixelSize / 2);
            }
        }
        private int _realMapHeight;
        private int _mapHeight
        {
            get
            {
                return _realMapHeight / (PixelSize / 2);
            }
        }
        private int _mapSize
        {
            get
            {
                return (_mapWidht * _mapHeight);
            }
        }
        private int _realMapSize
        {
            get
            {
                return (_realMapWidth * _realMapHeight);
            }
        }

        private GraphicsDevice _graphicDevice;

        private int _simulationStepsMS;
        private float _previousTime;
        private float _currentTime;
        private float _leftOverDeltaTime = 0;
        private float _ticksDone;

        private int _maxMass = 255;
        private int _minMass = 1;
        private int _maxCompress = 1;

        //private int _minDraw = 1;
        //private int _maxDraw = 200;
        private int _maxSpeed = 200;   //max units of water moved out of one block to another, per timestep
        private int _minFlow = 1;

        private int _pixelSize;

        private Rectangle _positionRect;

        public Fluids(int MapWidth, int MapHeight, GraphicsDevice GD, int SimulationSpeedInMS = 32)
        {
            _graphicDevice = GD;
            _pixelSize = 8;
            _realMapWidth = MapWidth;
            _realMapHeight = MapHeight;
            _mass = new byte[_mapSize];
            _newmass = new byte[_mapSize];
            _colorData = new Color[_realMapSize];
            _currentTexture = new Texture2D(_graphicDevice, _mapWidht, _mapHeight);


            _currentTexture.SetData<Color>(_colorData);

            _simulationStepsMS = SimulationSpeedInMS;

            _ticksDone = 0;

            _positionRect = new Rectangle(0, 0, _realMapWidth, _realMapHeight);

            ActiveHandler.Instance.Add(this);
        }

        public bool Active
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        //public int MaxDraw
        //{
        //    get
        //    {
        //        return _maxDraw;
        //    }
        //
        //    set
        //    {
        //        _maxDraw = value;
        //    }
        //}

        public int PixelSize
        {
            get
            {
                return _pixelSize;
            }

            set
            {
                _pixelSize = value;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (_currentTexture != null)
                sb.Draw(_currentTexture, _positionRect, Color.White);
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            //_currentMap = Terrain.Instance.CurrentMap;
            _currentTexture.SetData<Color>(_colorData);
            SimulateWater(currentGameTime);

        }

        private void SimulateWater(GameTime currentGameTime)
        {

            _currentTime = currentGameTime.TotalGameTime.Milliseconds;
            float deltaTimeMS = _currentTime - _previousTime; // how much time has elapsed since the last update
            _previousTime = _currentTime; // reset previousTime
            if (deltaTimeMS < 0)
                deltaTimeMS = 0;
            _ticksDone += deltaTimeMS;


            int timeStepAmt = 0;
            if (_ticksDone > _simulationStepsMS)
            {
                timeStepAmt = (int)Math.Floor(_ticksDone / _simulationStepsMS);
                _ticksDone = 0;
            }
            timeStepAmt = MathHelper.Min(timeStepAmt, 1);

            //for each Timestep do
            for (int iteration = 1; iteration <= timeStepAmt; iteration++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    for (int x = 0; x < _mapWidht; x++)
                    {
                        int Index = x + y * _mapWidht;
                        int BottomIndex = x + ((y + 1) * _mapWidht);
                        int UpIndex = x + ((y - 1) * _mapWidht);
                        int LeftIndex = Index - 1;
                        int RightIndex = Index + 1;

                        byte Flow = 0;
                        byte remainingmass = _mass[Index];

                        if (remainingmass <= 0)
                            continue;

                        if (!IsSolid(x, y + 1) && BottomIndex < _mapSize && _mass[BottomIndex] < _maxMass)
                        {
                            Flow = Subtract(GetStableWaterState(Add(remainingmass, _mass[BottomIndex])), _mass[BottomIndex]);
                            Flow = constrain(Flow, 0, Math.Min(_maxSpeed, (int)remainingmass));

                            _newmass[Index] = Subtract(_mass[Index], Flow);
                            _newmass[BottomIndex] = Add(_mass[BottomIndex], Flow);
                            remainingmass -= Convert.ToByte(MathHelper.Clamp(Flow, 1, 255));

                            ExtendColorData(Index);
                            ExtendColorData(BottomIndex);

                            //    SetArrayValue(ref _newmass, x, y + 1, GetArrayValue(_mass, x, y + 1) + Flow / 2);
                            //    SetData(x, y + 1);
                            //    remainingmass -= Flow / 2;
                        }

                        if (remainingmass <= 0)
                            continue;

                        if (!IsSolid(x - 1, y) && LeftIndex > 0 && _mass[LeftIndex] < _maxMass)
                        {
                            Flow = Convert.ToByte(Subtract(_mass[Index], _mass[LeftIndex]));
                            Flow = constrain(Flow, 0, Math.Min(_maxSpeed, (int)remainingmass));

                            _newmass[Index] = Subtract(_mass[Index], Flow);
                            _newmass[LeftIndex] = Add(_mass[BottomIndex], Flow);
                            remainingmass -= Convert.ToByte(MathHelper.Clamp(Flow, 1, 255));

                            ExtendColorData(Index);
                            ExtendColorData(LeftIndex);
                        }

                        if (remainingmass <= 0)
                            continue;

                        if (!IsSolid(x + 1, y) && RightIndex > _mapSize && _mass[RightIndex] < _maxMass)
                        {
                            Flow = Convert.ToByte(Subtract(_mass[Index], _mass[LeftIndex]));
                            Flow = constrain(Flow, 0, Math.Min(_maxSpeed, (int)remainingmass));

                            _newmass[Index] = Subtract(_mass[Index], Flow);
                            _newmass[RightIndex] = Add(_mass[BottomIndex], Flow);
                            remainingmass -= Convert.ToByte(MathHelper.Clamp(Flow, 1, 255));

                            ExtendColorData(Index);
                            ExtendColorData(RightIndex);
                        }

                        if (remainingmass <= 0)
                            continue;


                    }
                }
                FinishUpMass();
            }
            //CreateNewImage();

        }

        private byte Add(byte b1, byte b2)
        {
            int NewValue = b1 + b2;
            if (NewValue > 255)
                NewValue = 255;
            return Convert.ToByte(NewValue);
        }

        private byte Subtract(byte b1, byte b2)
        {
            int NewValue = b1 - b2;
            if (NewValue < 0)
                NewValue = 0;
            return Convert.ToByte(NewValue);
        }

        private byte constrain(int baseValue, int MinValue, int MaxValue)
        {
            int Return = baseValue;
            if (baseValue < MinValue)
                Return = MinValue;
            if (baseValue > MaxValue)
                Return = MaxValue;
            return Convert.ToByte(Return);
        }

        private byte GetStableWaterState(byte totalmass)
        {
            int ReturnValue;
            if (totalmass <= 1)
            {
                return 1;
            }
            else if (totalmass < 2 * _maxMass + _maxCompress)
            {
                ReturnValue = (_maxMass * _maxMass + totalmass * _maxCompress) / (_maxMass + _maxCompress);
            }
            else
            {
                ReturnValue = (totalmass + _maxCompress) / 2;
            }
            return Convert.ToByte(MathHelper.Clamp(ReturnValue, 0, 255));
        }

        private void FinishUpMass()
        {

            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidht; x++)
                {
                    int Index = x + y * _mapWidht;
                    _mass[Index] = _newmass[Index];
                }
            }
        }

        //private void CreateImage()
        //{
        //    _currentTexture = new Texture2D(_graphicDevice, _mapWidht, _mapHeight);
        //    Color[] colorData = new Color[_mapSize];
        //    _currentTexture.GetData<Color>(colorData);
        //
        //    for (int y = 0; y < _mapHeight; y++)
        //    {
        //        for (int x = 0; x < _mapWidht; x++)
        //        {
        //            int iPixelIndex = x + _mapWidht * y;
        //            colorData[iPixelIndex] = Color.Transparent;
        //        }
        //    }
        //
        //    _currentTexture.SetData<Color>(colorData);
        //}

        private void ExtendColorData(int Index)
        {
            Color ColorToUse = Color.Transparent;
            if (_newmass[Index] > 0)
                ColorToUse = Color.Blue;
            _colorData[Index] = ColorToUse;



            //_currentTexture.SetData<Color>(_colorData); 
        }

        public bool SpawnWater(int x, int y, byte Amount)
        {
            if (IsSolid(x, y))
                return false;
            _mass[x + y * _mapWidht] += Amount;
            //byte _water = GetArrayValue(_mass, x, y);
            //SetArrayValue(ref _mass, x, y, Amount + _water);
            return true;
        }

        private float GetArrayValue(float[] array, int x, int y)
        {
            if (array == null)
                return 0f;
            return array[y * _mapWidht + x];
        }

        private void SetArrayValue(ref float[] array, int x, int y, float NewValue)
        {
            if (array == null)
                return;
            array[y * _mapWidht + x] = NewValue;
        }

        private bool IsSolid(int x, int y)
        {
            if (x > _mapWidht - 1 || y > _mapHeight - 1)
                return true;

            if (x < 0 || y < 0)
                return true;

            int StartIndexX = x * (_pixelSize / 2);
            int StartIndexY = y * (_pixelSize / 2);
            bool Solid = false;
            for (int ly = 0; ly < (_pixelSize / 2); ly++)
            {
                for (int lx = 0; lx < (_pixelSize / 2); lx++)
                {
                    int Index = (StartIndexX + lx) + ((StartIndexY + ly) * _realMapWidth);
                    if (Index > _realMapSize)
                        continue;
                    Solid = Settings.BackgroundPixels[Index] == Color.Transparent ? false : true;
                    if (Solid)
                        return Solid;
                }
            }

            return Solid;
        }

        public void Dispose()
        {
            ActiveHandler.Instance.Remove(this);
        }

        ~Fluids()
        {
            ActiveHandler.Instance.Remove(this);
        }
    }
}
