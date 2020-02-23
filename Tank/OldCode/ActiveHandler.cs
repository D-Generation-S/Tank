using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tank.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Tank.Code.Entities;
using System.Linq;

namespace Tank.Code
{
    public class ActiveHandler
    {
        public static ActiveHandler _instance;
        public static ActiveHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ActiveHandler();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public List<IActiveObj> objects;
        public List<Player> player;

        public PlayerIndex CurrentPlayer
        {
            get
            {
                Player CurrentPlayer = GetActivePlayer();
                if (CurrentPlayer == null)
                    return PlayerIndex.One;
                else
                    return CurrentPlayer.Index;
            }
        }

        private Player _currentActivePlayer;
        public Player CurrentActivePlayer
        {
            get
            {
                return _currentActivePlayer;
            }
        }

        private Player GetActivePlayer()
        {
            foreach (Player item in player)
            {
                if (item.Active)
                {
                    return item;
                }
            }
            return null;
        }

        public ActiveHandler()
        {
            objects = new List<IActiveObj>();
            player = new List<Player>();
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                IActiveObj obj = objects[i];
                obj.Update(mouseState, keyboardState, currentGameTime, gsState);
            }

        }

        public void Add(IActiveObj obj)
        {
            objects.Add(obj);
            if (obj.GetType() == typeof(Player))
            {
                Player CurrentPlayer = (Player)obj;
                CurrentPlayer.Finish += CurrentPlayer_Finish;
                player.Add(CurrentPlayer);
            }
        }

        public void InitializeNewRound()
        {
            _currentActivePlayer = GetActivePlayer();
        }

        public void EndRound()
        {
            for (int i = objects.Count - 1; i > 0; i--)
            {
                if (objects[i].GetType() == typeof(Player))
                {
                    objects.RemoveAt(i);
                }
            }
            player.Clear();
            Renderer.Instance.EndRound();
        }

        private void CurrentPlayer_Finish(object sender, EventArgs e)
        {
            Player CurrentPlayer = (Player)sender;
            CurrentPlayer.Active = false;
            int StartPos = 0;
            int CurrentPosition = player.IndexOf(CurrentPlayer);
            if (CurrentPosition < player.Count - 1)
                StartPos = CurrentPosition + 1;
            for (int i = StartPos; i < player.Count; i++)
            {
                if (i == CurrentPosition)
                    break;

                if (player[i].GetType() == typeof(Player))
                {
                    if (player[i].IsAlive)
                    {
                        player[i].Active = true;
                        break;
                    }
                }
            }
        }

        public void Remove(IActiveObj obj)
        {
            objects.Remove(obj);
        }

        public static void SingleProjectile_HitEvent(object sender, EventArgs e)
        {
            Projectile singleProjectile = sender as Projectile;
            ProjectileHitEventArgs args = (ProjectileHitEventArgs)e;

            string damage = "No Hit";

            List<IActiveObj> player = Instance.objects.Where(obj => obj.GetType() == typeof(Player) && ((Player)obj).IsAlive).ToList();

            for (int i = 0; i <= player.Count - 1; i++)
            {
                IActiveObj item = player[i];
                Player p = (Player)item;
                Rectangle pRect = p.TankSize;
                Rectangle expRect = new Rectangle((int)(args.X - args.Radius), (int)(args.Y - args.Radius), args.Diameter, args.Diameter);



                if (expRect.Intersects(pRect))
                {
                    int Damage = CalculateDamage((int)args.X, (int)args.Y, p.TankSize, args.Diameter, args.Damage);
                    damage = $"DMG: {Damage}";
                    p.Hit(Damage);
                    if (singleProjectile.Caller == p)
                    {
                        singleProjectile.Caller.Money -= 20;
                    }
                    else
                    {
                        singleProjectile.Caller.Money += 10;
                    }
                    if (!p.IsAlive)
                    {
                        if (Settings.DeathSound != null)
                            Settings.DeathSound.Play(Settings.SoundVolume, 0, 0);
                        singleProjectile.Caller.Money += 40;
                    }

                    if (Settings.HitSound != null)
                        Settings.HitSound.Play(Settings.SoundVolume, 0, 0);
                }
            }
            DebugEntity ExplosionCircle = new DebugEntity((int)args.X, (int)args.Y, CreateExplosionCircle((args.Diameter / 2) / 3, args.Diameter / 2), damage);

            singleProjectile.Caller.TriggerFinishEvent();

            singleProjectile.Dispose();
            singleProjectile = null;
        }

        private static int CalculateDamage(int ExpCx, int ExpCy, Rectangle playerRect, int Diameter, int CenterDamage)
        {
            float radius = Diameter / 2;
            float FullHitRadius = radius / 3;
            int damageDone = 0;

            Vector2[] points = new Vector2[] {new Vector2(playerRect.X, playerRect.Y),
                new Vector2(playerRect.X + playerRect.Width, playerRect.Y),
                new Vector2(playerRect.X, playerRect.Y + playerRect.Height),
                new Vector2(playerRect.X + playerRect.Width, playerRect.Y + playerRect.Height) };

            for (int i = 0; i < points.Length; i++)
            {
                float DamageDistance = (float)Math.Sqrt(Math.Pow((points[i].X - ExpCx), 2) + Math.Pow((points[i].Y - ExpCy), 2));
                if (DamageDistance < radius)
                {
                    DebugEntity hitPoint = new DebugEntity((int)points[i].X, (int)points[i].Y, TankGame.PublicContentManager.Load<Texture2D>("Pixel_Red"), string.Empty, 5000, 10);
                    if (DamageDistance < radius)
                        damageDone += CenterDamage;
                    else
                    {
                        DamageDistance -= FullHitRadius;
                        float MaxDistance = radius - FullHitRadius;

                        float ProceturalDamage = (DamageDistance / MaxDistance);

                        damageDone += (int)(CenterDamage * ProceturalDamage);
                    }
                }
            }
            if (damageDone > 0)
                damageDone /= 4;
            return damageDone;
        }

        private static Texture2D CreateExplosionCircle(float InnerRadius, float OuterRadius)
        {
            int LargeRadius = (int)OuterRadius;
            int SmallRadius = (int)InnerRadius;
            int ImageSize = LargeRadius * 2 + 2;
            Vector2 CircleCenter = new Vector2(ImageSize / 2, ImageSize / 2);
            Texture2D Picture = new Texture2D(TankGame.PublicGraphicsDevice, ImageSize, ImageSize);
            Color[] colorData = new Color[ImageSize * ImageSize];

            Color InnerCircle = new Color(Color.Red, 10);
            Color OuterCircle = new Color(Color.Yellow, 10);



            for (int y = 0; y < ImageSize; y++)
            {
                for (int x = 0; x < ImageSize; x++)
                {
                    int Index = x + y * ImageSize;
                    if (PointInCircle((int)CircleCenter.X, (int)CircleCenter.Y, x, y, LargeRadius))
                        colorData[Index] = OuterCircle;

                }
            }

            for (int y = 0; y < ImageSize; y++)
            {
                for (int x = 0; x < ImageSize; x++)
                {
                    int Index = x + y * ImageSize;
                    if (PointInCircle((int)CircleCenter.X, (int)CircleCenter.Y, x, y, SmallRadius))
                        colorData[Index] = InnerCircle;


                }
            }

            Picture.SetData<Color>(colorData);
            return Picture;
        }

        private static bool PointInCircle(int Cx, int Cy, int Px, int Py, float Radius)
        {
            float TestValue = (float)Math.Sqrt(Math.Pow((Px - Cx), 2) + Math.Pow((Py - Cy), 2));
            if (TestValue < Radius)
                return true;
            return false;
        }
    }
}
