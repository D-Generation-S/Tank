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
    class Projectile : DrawableEntity, IPhysicEntity
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

        private Position lastPosition;

        public Projectile(IRenderer renderer) : base(renderer)
        {
            onGround = true;
        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            lastPosition = new Position((int)Position.X, (int)Position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rotation = (float)Math.Atan2(Position.Y - lastPosition.Y, Position.X - lastPosition.X);
            lastPosition = new Position((int)Position.X, (int)Position.Y);
        }
    }
}
