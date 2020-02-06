using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.GameManager
{ 
    class DefaultEntitySystem : BaseEntity, IEntitySystem
    {
        private readonly List<IEntity> entities;
        public IList<IEntity> Entities => entities;

        public DefaultEntitySystem()
        {
            entities = new List<IEntity>();
            Initzialize(Guid.NewGuid().ToString());
        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            active = true;
            alive = true;
        }

        public string AddEntity(IEntity entity)
        {
            string name = Guid.NewGuid().ToString();
            entity.Initzialize(name);

            foreach (IEntity systemEntity in entities)
            {
                if (systemEntity is ISystem)
                {
                    ((ISystem)systemEntity).AddEntity(entity);
                }
            }
            entities.Add(entity);

            return name;
        }

        public bool RemoveEntity(string entityName)
        {
            int counter = entities.RemoveAll(entity => entity.UniqueName == entityName);

            bool removingOkay = true;
            foreach (IEntity systemEntity in entities)
            {
                if (systemEntity is ISystem)
                {
                    removingOkay &= ((ISystem)systemEntity).RemoveEntity(entityName);
                }
            }

            return counter > 0 && removingOkay;
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is IDrawableEntity && entities[i].Active)
                {
                    ((IDrawableEntity)entities[i]).Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CleanEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Active)
                {
                    entities[i].Update(gameTime);
                }
            }
        }

        private void CleanEntities()
        {
            for (int i = entities.Count - 1; i > 0; i--)
            {
                if (!entities[i].Alive)
                {
                    RemoveEntity(entities[i].UniqueName);
                }
            }
        }
    }
}
