using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Entities.Weapon
{
    class Projectile : BaseEntity, IVisibleEntity, IPhysicEntity
    {
        private IRenderer renderer;
        public IRenderer Renderer => renderer;

        private Vector2 position;
        public Vector2 Position {
            get => position;
            set => position = value; 
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
        
        public Projectile(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            alive = true;
            base.Initzialize(uniqueName);
        }

        public override void Update(GameTime gameTime)
        {
            renderer.Position = position;
        }
    }
}
