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
        private readonly List<IEntity> updateableEntities;
        public IList<IEntity> UpdateableEntities => updateableEntities;

        private readonly List<IDrawableEntity> drawableEntities;
        public IList<IDrawableEntity> DrawableEntities => drawableEntities;

        private readonly List<ISystem> systems;
        public IList<ISystem> Systems => systems;

        public DefaultEntitySystem()
        {
            updateableEntities = new List<IEntity>();
            drawableEntities = new List<IDrawableEntity>();
            systems = new List<ISystem>();
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

            foreach (ISystem system in systems)
            {
                system.AddEntity(entity);
            }

            updateableEntities.Add(entity);

            if (entity is IDrawableEntity)
            {
                drawableEntities.Add((IDrawableEntity)entity);
            }

            if (entity is ISystem)
            {
                systems.Add((ISystem)entity);
            }

            return name;
        }

        public bool RemoveEntity(string entityName)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < drawableEntities.Count; i++)
            {
                if (drawableEntities[i].Active)
                {
                    drawableEntities[i].Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CleanEntities();
            for (int i = 0; i < updateableEntities.Count; i++)
            {
                if (updateableEntities[i].Active)
                {
                    updateableEntities[i].Update(gameTime);
                }
            }
        }

        private void CleanEntities()
        {
            for (int i = updateableEntities.Count - 1; i > 0; i--)
            {
                if (!updateableEntities[i].Alive)
                {
                    updateableEntities.RemoveAt(i);
                }
            }

            for (int i = drawableEntities.Count - 1; i > 0; i--)
            {
                if (!drawableEntities[i].Alive)
                {
                    drawableEntities.RemoveAt(i);
                }
            }

            for (int i = systems.Count - 1; i > 0; i--)
            {
                if (!systems[i].Alive)
                {
                    systems.RemoveAt(i);
                }
            }
        }
    }
}
