using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Projectile : IPhysicsObj, IDisposable
    {
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

        private float lastX, lastY;
        private Animation bullet;

        public event EventHandler HitEvent;

        public Player Caller;

        public Projectile(float x, float y, float vX, float vY)
        {
            this.x = x;
            this.y = y;
            lastX = x;
            lastY = y;
            velX = vX;
            velY = vY;

            Physics.Instance.Add(this);
            if (Settings.BasicMunitionSprite != null)
                LoadContent(Settings.BasicMunitionSprite);
        }

        public void LoadContent(Texture2D t2D)
        {
            bullet = new Animation(t2D, 32, 32, 0.05f);
        }

        public void checkConstraints(GameTime CurrentGameTime)
        {
            if (Terrain.Instance == null)
                return;

            // Boundary constraints... only remove the pixel if it exits the sides or bottom of the map
            //y > Terrain.Instance.Height || <-- Proejectiles should always hit if outside top
            if ((x > Terrain.Instance.Width) || x < 0 || y < -Terrain.Instance.Height)
            {
                TriggerHitEvent(false);
                return;
            }
            int[] collision = Helper.RayCast((int)lastX, (int)lastY, (int)x, (int)y);
            if (collision.Length > 0 || y > Terrain.Instance.Height || HitPlayer())
            {
                TriggerHitEvent();
            }

            if (bullet != null)
            {
                
                bullet.Rotation = (float)Math.Atan2(y - lastY, x - lastX);

                bullet.X = (int)x;
                bullet.Y = (int)y;
            }
            lastX = x;
            lastY = y;
        }

        private bool HitPlayer()
        {
            foreach (Player CurrentPlayer in ActiveHandler.Instance.player.Where(player => player.IsAlive))
            {
                Rectangle rect = CurrentPlayer.TankSize;
                if (rect.Contains(x, y))
                    return true;
            }
            return false;
        }

        private void TriggerHitEvent(bool TerrainHit = true)
        {
            if (TerrainHit)
            {
                Explode exp = new Explode((int)x, (int)y, 60);
            }
            ProjectileHitEventArgs e = new ProjectileHitEventArgs()
            {
                X = x,
                Y = y,
                Hit = TerrainHit,
                Damage = 25,
                Diameter = 100,
            };
            bullet.Dispose();
            Physics.Instance.Remove(this);
            HitEvent(this, e);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (bullet != null)
                {
                    bullet = null;
                }
            }
        }

        ~Projectile()
        {
            Dispose(false);
        }
    }

    internal class ProjectileHitEventArgs : EventArgs
    {
        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get; set;
        }

        public int Diameter
        {
            get;
            set;
        }

        public float Radius
        {
            get
            {
                return Diameter / 2;
            }
        }

        public bool Hit
        {
            get; set;
        }

        public int Damage
        {
            get; set;
        }
    }
}