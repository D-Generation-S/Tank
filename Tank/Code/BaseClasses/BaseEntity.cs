using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Entity;

namespace Tank.Code.BaseClasses
{
    class BaseEntity : IEntity
    {
        private string uniqueName;
        public string UniqueName => uniqueName;

        protected bool active;
        public bool Active => active;

        protected bool alive;
        public bool Alive => alive;

        public virtual void Initzialize(string uniqueName)
        {
            this.uniqueName = uniqueName;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
