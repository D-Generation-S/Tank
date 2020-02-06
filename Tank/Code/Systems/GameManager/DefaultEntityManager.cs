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
    class DefaultEntityManager : BaseEntity, IEntityManager
    {
        private readonly List<IEntity> updateableEntities;
        public IList<IEntity> UpdateableEntities => updateableEntities;

        private readonly List<IDrawableEntity> drawableEntities;
        public IList<IDrawableEntity> DrawableEntities => drawableEntities;

        public DefaultEntityManager()
        {
            updateableEntities = new List<IEntity>();
            drawableEntities = new List<IDrawableEntity>();
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


            updateableEntities.Add(entity);

            if (entity is IDrawableEntity)
            {
                drawableEntities.Add((IDrawableEntity)entity);
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
            for (int i = updateableEntities.Count; i > 0; i--)
            {
                if (!updateableEntities[i].Alive)
                {
                    updateableEntities.RemoveAt(i);
                }
            }

            for (int i = drawableEntities.Count; i > 0; i--)
            {
                if (!drawableEntities[i].Alive)
                {
                    drawableEntities.RemoveAt(i);
                }
            }
        }
    }
}
