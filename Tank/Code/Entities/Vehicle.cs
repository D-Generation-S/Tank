using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Code.GUIClasses;
using Tank.Interfaces;

namespace Tank.Code.Entities
{
    public class Vehicle : IPhysicsObj, IRenderObj, IDisposable
    {

        private bool _topBlocked;

        private float maxRotation;
        private float minRotation;


        private TimeSpan _elapsedGameTime;

        public float velX
        {
            get;
            set;
        }
        public float velY
        {
            get;
            set;
        }

        public float x
        {
            get;
            set;
        }
        public float y
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public float CurrentPower
        {
            get;
            set;
        }

        public bool IsOnGround;

        public float MinPower = 10;
        public float MaxPower = 100;

        public Texture2D Canon;
        public Texture2D Body;

        public float Rotation
        {
            get;
            private set;
        }

        public float CanonX
        {
            get
            {
                return x + Width / 2;
            }
        }
        public float CanonY
        {
            get
            {
                return y + Canon.Height;
            }
        }

        private float _startingArmor = 0;
        private float _Armor;
        public float Armor
        {
            get
            {
                return _Armor;
            }
            set
            {
                if (_startingArmor <= 0)
                    _startingArmor = value;
                if (life != null)
                {
                    life.Progress = (value / (_startingArmor / 100)) / 100;
                    if (value < _startingArmor)
                        life.Hidden = false;
                }

                _Armor = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(x, y);
            }
        }
        private GUIProgressBar life;

        public Vehicle()
        {
            Rotation = (float)(Math.PI * -0.5);
            minRotation = (float)(Math.PI * -1);
            maxRotation = 0;
        }

        public void Initialize()
        {
            Texture2D backTexture = TankGame.PublicContentManager.Load<Texture2D>("Images/Assets/LifeBar_Background");
            float ratio = (float)backTexture.Height / (float)backTexture.Width;
            int iHeight = (int)(Width * ratio);
            life = new GUIProgressBar((int)x, (int)y + Height + 20, Width, iHeight, backTexture, TankGame.PublicContentManager.Load<Texture2D>("Images/Assets/LifeBar_Foreground"));
            life.Progress = 1;
            life.Hidden = true;
            Renderer.Instance.Add(this);
            Physics.Instance.Add(this);
        }

        public void SetRotation(float difference)
        {
            float Harderpercent = 1 - (CurrentPower - MinPower) / (MaxPower - MinPower);
            if (Harderpercent <= 0)
                Harderpercent = 0.01f;
            float RotationDifference = difference * Harderpercent;
            Rotation = MathHelper.Clamp((Rotation + RotationDifference), minRotation, maxRotation);
        }

        public void SetPower(float difference)
        {
            CurrentPower = MathHelper.Clamp((CurrentPower + difference), MinPower, MaxPower);
        }

        public void checkConstraints(GameTime CurrentGameTime)
        {
            if (Terrain.Instance == null)
                return;

            _topBlocked = false;
            // start with the top edge
            for (int topX = (int)x - Width / 2; topX <= (int)x + Width / 2; topX++)
            {
                if (Terrain.IsPixelSolid(topX, (int)y - Height / 2 - 1))
                { // if the pixel is solid
                    _topBlocked = true;
                    if (velY < 0)
                    {
                        velY *= -0.5f;
                    }
                }
            }
            // loop left edge
            if (velX < 0)
            {
                for (int leftY = (int)y - Height / 2; leftY <= (int)y + Height / 2; leftY++)
                {
                    if (Terrain.IsPixelSolid((int)x - Width / 2, leftY))
                    {
                        // next move from the edge to the right, inside the box (stop it at 1/4th the player width)
                        for (int xCheck = (int)x - Width / 4; xCheck < (int)x - Width / 2; xCheck--)
                        {
                            if (Terrain.IsPixelSolid(xCheck, leftY))
                            {
                                x = xCheck + Width / 2; // push the block over 
                                break;
                            }
                        }
                        if (leftY > y && !_topBlocked)
                        {
                            y -= 1;
                        }
                        else
                        {
                            velX *= -0.4f;
                            x += 2;
                        }
                    }
                }
            }
            // do the same for the right edge
            if (velX > 0)
            {
                for (int rightY = (int)y - Height / 2; rightY <= (int)y + Height / 2; rightY++)
                {
                    if (Terrain.IsPixelSolid((int)x + Width / 2, rightY))
                    {
                        for (int xCheck = (int)x + Width / 4; xCheck < (int)x + Width / 2 + 1; xCheck++)
                        {
                            if (Terrain.IsPixelSolid(xCheck, rightY))
                            {
                                x = xCheck - Width / 2;
                                break;
                            }
                        }
                        if (rightY > y && !_topBlocked)
                        {
                            y -= 1;
                        }
                        else
                        {
                            velX *= -0.4f;
                            x -= 2;
                        }
                    }
                }
            }
            IsOnGround = false;
            for (int bottomX = (int)x - Width / 2; bottomX <= (int)x + Width / 2; bottomX++)
            {
                if (Terrain.IsPixelSolid(bottomX, (int)y + Height / 2 + 1) && (velY > 0))
                {
                    IsOnGround = true;
                    for (int yCheck = (int)y + Height / 4; yCheck < (int)y + Height / 2; yCheck++)
                    {
                        if (Terrain.IsPixelSolid(bottomX, yCheck))
                        {
                            y = yCheck - Height / 2;
                            break;
                        }
                    }
                    if (velY > 0)
                        velY *= -0.25f;
                }
            }
            // Boundary Checks
            if (x < 0 && velX < 0)
            {
                x -= x;
                velX *= -1;
            }
            if (y < 0 && velY < 0)
            {
                y -= y;
                velY *= -1;
            }
            if (x > Terrain.Instance.Width && velX > 0)
            {
                x += Terrain.Instance.Width - x;
                velX *= -1;
            }
            if (y + Height / 2 > Terrain.Instance.Height && velY > 0)
            {
                y += Terrain.Instance.Height - y - Height / 2;
                velY = 0;
                IsOnGround = true;
            }

            life.X = (int)x;
            life.Y = (int)y + Height + 20;
            _elapsedGameTime = CurrentGameTime.TotalGameTime;
        }

        public void Draw(SpriteBatch sb)
        {            
            sb.Draw(Body, new Rectangle((int)x, (int)y, Width, Height), Color.White);
            sb.Draw(Canon, new Vector2(CanonX, CanonY), null, Color.White, Rotation, new Vector2(2, Canon.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public Vector2 CalculateCanonEnd()
        {
            return new Vector2(((float)Math.Cos(Rotation) * Canon.Width) + CanonX, ((float)Math.Sin(Rotation) * Canon.Width) + CanonY);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Renderer.Instance.Remove(this);
                Physics.Instance.Remove(this);
                life.Dispose();
            }
        }
        ~Vehicle()
        {
            Dispose(false);
        }
    }
}