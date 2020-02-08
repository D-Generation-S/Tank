using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Code.DataContainer;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Entities.Weapon
{
    class Projectile : DrawableEntity, IPhysicEntity, ICollideableEntity
    {
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        private bool onGround;
        public bool OnGround
        {
            get => onGround;
            set => onGround = value;
        }

        protected bool mapCollision;
        public bool MapCollision => mapCollision;

        private Rectangle collider;
        public Rectangle Collider => collider;

        private Position lastPosition;

        public Projectile(IRenderer renderer) : base(renderer)
        {
        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            collider = new Rectangle((int)Position.X, (int)Position.Y, (int)Renderer.Size.X, (int)Renderer.Size.Y);
            mapCollision = true;
            lastPosition = new Position((int)Position.X, (int)Position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!onGround)
            {
                rotation = (float)Math.Atan2(Position.Y - lastPosition.Y, Position.X - lastPosition.X);
            }
            

            if (lastPosition.X != (int)Position.X || lastPosition.Y != (int)Position.Y) 
            {
                collider.X = (int)Position.X;
                collider.Y = (int)Position.Y;
            }

            lastPosition = new Position((int)Position.X, (int)Position.Y);
        }
    }
}
