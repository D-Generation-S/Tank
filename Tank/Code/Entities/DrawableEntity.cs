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

namespace Tank.Code.Entities
{
    class DrawableEntity : BaseEntity, IVisibleEntity
    {
        private IRenderer renderer;
        public IRenderer Renderer => renderer;

        private Vector2 position;
        public Vector2 Position => position;

        
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public DrawableEntity()
        {
        }

        public void Initialize()
        {
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Initzialize()
        {
            throw new NotImplementedException();
        }
    }
}
