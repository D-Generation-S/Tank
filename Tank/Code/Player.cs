using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Tank.Code.Entities;
using Tank.Code.General;
using Tank.Code.GUIClasses;
using Tank.Code.JSonClasses;
using Tank.Code.SubClasses;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Player : IActiveObj, IDisposable
    {
        private Vehicle _tank;

        private float graphicUpdateTime = 50;
        private float lastGraphicUpdateTime = 0;

        private bool _active;

        private PlayerOverlay overlay;

        private TimeSpan PrimaryFireTime;
        private TimeSpan SecondaryFireTime;
        private GameTime previousGameTime;
        private Animation activityIndicator;

        private bool ShootAlowed;


        private string _playername;
        public string PlayerName
        {
            get
            {
                return _playername;
            }
        }
        
        public int Money
        {
            get;
            set;
        }


        public event EventHandler Finish;

        public PlayerIndex Index;


        public bool IsAlive
        {
            get
            {
                return _tank.Armor > 0;
            }
        }

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                ShootAlowed = value;
            }
        }

        public Rectangle TankSize
        {
            get
            {
                return new Rectangle((int)_tank.x, (int)_tank.y, _tank.Width, _tank.Height);
            }
        }
        public int PositionX
        {
            get
            {
                return (int)_tank.Position.X;
            }
        }
        public int PositionY
        {
            get
            {
                return (int)_tank.Position.Y;
            }
        }

        private int _id;

        private Position _spawnPostion;

        public Player(int X, int Y, int PIndex, Vehicle TankToUse, string playerName = null)
        {
            //string tankJson = CodeHelper.LoadJson("Entities\\Tanks\\heavy");
            //
            //jsonTank tankDefinition = JsonConvert.DeserializeObject<jsonTank>(tankJson);
            _tank = TankToUse;
            _spawnPostion = new Position()
            {
                X = X,
                Y = Y,
            };
            _id = PIndex;
            _playername = playerName;
            
            SecondaryFireTime = new TimeSpan(0, 0, 0, 0, 15);
            PrimaryFireTime = new TimeSpan(0, 0, 0, 0, 200);
            if (PIndex <= 4)
                Index = (PlayerIndex)PIndex;

        }

        public void Initialize(int zoneWidth, Random seed)
        {
            Position SpawnPos = GetSpawnPosition(zoneWidth, seed);
            Spawn(SpawnPos.X, SpawnPos.Y);

            _tank.Initialize();

            ActiveHandler.Instance.Add(this);
        }

        private Position GetSpawnPosition(int zoneW, Random seed)
        {
            return new Position()
            {
                X = (int)(seed.Next(zoneW * _id + 32, zoneW * _id + zoneW + 32)),
                Y = 20,
            };
        }

        public void LoadContent(Texture2D meter, Texture2D bar, Texture2D healthFront, Texture2D healthBack)
        {
            overlay?.LoadContent();
        }

        private void Shoot()
        {
            if (ShootAlowed)
            {
                ShootAlowed = false;
                Vector2 canonEnd = _tank.CalculateCanonEnd();
                float velX = (canonEnd.X - _tank.CanonX) * overlay.CurrentPower;
                float velY = (canonEnd.Y - _tank.CanonY) * overlay.CurrentPower;
                Projectile singleProjectile = new Projectile(canonEnd.X, canonEnd.Y, velX, velY);
                singleProjectile.Caller = this;
                singleProjectile.HitEvent += ActiveHandler.SingleProjectile_HitEvent;
                if (Settings.ShootSound != null)
                    Settings.ShootSound.Play(Settings.SoundVolume, 0, 0);
            }
        }

        public void TriggerFinishEvent()
        {
            EventHandler handler = Finish;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
        public void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (!Active)
            {
                activityIndicator?.Dispose();
                activityIndicator = null;
                overlay?.Dispose();
                overlay = null;
                return;
            }
            if (overlay == null)
            {
                overlay = new PlayerOverlay(_id);
            }
            if (activityIndicator == null)
            {
                activityIndicator = new Animation(TankGame.PublicContentManager.Load<Texture2D>("Images/Assets/Selection_Arrow"), 32, 32, 0.05f, 1)
                {
                    X = (int)_tank.x + _tank.Width / 2,
                    Y = (int)_tank.y - (_tank.Height + _tank.Canon.Width + 10)
                };
            }
            lastGraphicUpdateTime += currentGameTime.ElapsedGameTime.Milliseconds;

            if (lastGraphicUpdateTime > graphicUpdateTime)
            {
                lastGraphicUpdateTime -= graphicUpdateTime;
                if (keyboardState.KeyPressed(Settings.GoRightKey) || (gsState != null && gsState.IsButtonDown(Settings.Right)))
                    _tank.SetRotation(0.1f);
                else if (keyboardState.KeyPressed(Settings.GoLeftKey) || (gsState != null && gsState.IsButtonDown(Settings.Left)))
                    _tank.SetRotation(-0.1f);

                if (keyboardState.KeyPressed(Settings.IncreasePower) || (gsState != null && gsState.IsButtonDown(Settings.IncPower)))
                    _tank.SetPower(0.5f);
                else if (keyboardState.KeyPressed(Settings.DecreasePower) || (gsState != null && gsState.IsButtonDown(Settings.DecPower)))
                    _tank.SetPower(-0.5f);
            }

            overlay.CurrentAngle = _tank.Rotation;
            overlay.CurrentPower = _tank.CurrentPower;

            if (keyboardState.KeyPressed(Settings.ShootKey) || (gsState != null && gsState.IsButtonDown(Settings.Shoot)))
                Shoot();

            previousGameTime = currentGameTime;

            if (Settings.Debug)
            {
                Vector2 canonEnd = _tank.CalculateCanonEnd();

                float velX = (canonEnd.X - _tank.CanonX) * overlay.CurrentPower;
                float velY = (canonEnd.Y - _tank.CanonY) * overlay.CurrentPower;

                overlay.CurrentBulletHitPoint = Helper.GetBulletTrajectoryLine(canonEnd.X, canonEnd.Y, velX, velY);
            }
        }

        public void Hit(float damage)
        {
            _tank.Armor -= damage;
            if (!IsAlive)
            {
                _tank.Dispose();
                Dispose(true);
            }
        }

        private void Spawn(float X, float Y)
        {
            float newPosY = Helper.GetGroundPosition(X, Y);
            newPosY -= _tank.Height;
            Move(X, newPosY);
        }

        public void Move(float x, float y)
        {
            _tank.x = x;
            _tank.y = y;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                activityIndicator?.Dispose();
                overlay?.Dispose();
                ActiveHandler.Instance.Remove(this);
            }
        }

        ~Player()
        {
            Dispose(false);
        }
    }
}